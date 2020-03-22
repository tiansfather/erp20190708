using Abp.Domain.Repositories;
using Abp.UI;
using Master.Authentication;
using Master.Entity;
using Master.Extensions;
using Master.Module;
using Master.Storage;
using Master.Units;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.FlowHandlers
{
    /// <summary>
    /// 实物订货单
    /// </summary>
    public class SDRFlowHandler : FlowHandlerBase
    {
        
        public MaterialManager MaterialManager { get; set; }
        public MaterialSellManager MaterialSellManager { get; set; }
        public MaterialSellBackManager MaterialSellBackManager { get; set; }
        public MaterialSellOutManager MaterialSellOutManager { get; set; }
        public StoreMaterialManager StoreMaterialManager { get; set; }
        public UnitManager UnitManager { get; set; }
       
        public IRepository<MaterialSellCart,int> CartRepository { get; set; }

        public override async Task Handle(FlowSheet flowSheet)
        {
            await base.Handle(flowSheet);
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];

            flowSheet.SheetDate = sheetHeader["sheetDate"].ToObjectWithDefault<DateTime>();
            flowSheet.Remarks = sheetHeader["remarks"].ToObjectWithDefault<string>();
            var unitId = sheetHeader["unitId"].ToObject<int>();//代理商id
            flowSheet.UnitId = unitId;
            flowSheet.OrderStatus="待审核";
            flowSheet.SetPropertyValue("OrderType", CurrentUser.IsCenterUser?"中心代为下单": "代理商自助下单");
            //清空对应代理商的实物购物车
            await CartRepository.DeleteAsync(o => o.CreatorUserId == AbpSession.UserId && o.UnitId == unitId && o.Material.MaterialNature == MaterialNature.实物);

            foreach (var sheetItem in sheetData["body"])
            {
                var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id
                
                var number= sheetItem["number"].ToObjectWithDefault<int>();//订购数量
                //记录销售
                var materialSell = new MaterialSell()
                {
                    MaterialId = materialId,
                    FlowSheetId = flowSheet.Id,
                    SellNumber = number,
                    UnitId=unitId
                };
                await MaterialSellManager.InsertAsync(materialSell);
                
            }

        }

        public override async Task HandleRevert(FlowSheet flowSheet)
        {
            
            
        }

        public override async Task<IEnumerable<ModuleButton>> GetFlowBtns(FlowSheet flowSheet)
        {
            var btns = new List<ModuleButton>();
            if (flowSheet.OrderStatus == "待审核")
            {
                btns.Add(new ModuleButton()
                {
                    ButtonKey = "backToCart",
                    ButtonName = "放回购物车修改",
                    ConfirmMsg="确认将订单放回购物车?此订单将失效"
                });
                if (CurrentUser.IsCenterUser)
                {
                    btns.Add(new ModuleButton()
                    {
                        ButtonKey = "verify",
                        ButtonName = "审核",
                        ConfirmMsg = "确认审核通过此订单？"
                    });
                    btns.Add(new ModuleButton()
                    {
                        ButtonKey = "cancel",
                        ButtonName = "取消",
                        ButtonClass = "layui-btn-danger",
                        ConfirmMsg = "确认取消此单据？"
                    });
                }
                
            }
            else if (flowSheet.OrderStatus == "待发货" && CurrentUser.IsCenterUser)
            {
                btns.Add(new ModuleButton()
                {
                    ButtonKey = "send",
                    ButtonName = "发货",
                    ConfirmMsg = "确认发货？"
                });
            }
            else if (flowSheet.OrderStatus == "已发货" && CurrentUser.IsCenterUser)
            {
                btns.Add(new ModuleButton()
                {
                    ButtonKey = "back",
                    ButtonName = "退货",
                    ConfirmMsg = "确认退货？"
                });
            }
            return btns;
        }

        public override async Task Action(FlowSheet flowSheet, string action)
        {
            //数据处理
            var formObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(flowSheet.FlowInstance.FormData);
            var sheetData = formObj["sheetData"];
            var sheetHeader = sheetData["header"];
            var unitId = sheetHeader["unitId"].ToObject<int>();//代理商id
            if (action == "backToCart")
            {
                foreach (var sheetItem in sheetData["body"])
                {
                    var materialId = Convert.ToInt32(sheetItem["id"]);//对应的物料Id
                    var number = sheetItem["number"].ToObjectWithDefault<int>();//订购数量

                    var materialCart = await CartRepository.GetAll().Where(o => o.UnitId == unitId && o.CreatorUserId == AbpSession.UserId && o.MaterialId == materialId).FirstOrDefaultAsync();
                    if (materialCart != null)
                    {
                        materialCart.Number += number;
                        await CartRepository.UpdateAsync(materialCart);
                    }
                    else
                    {
                        materialCart = new MaterialSellCart()
                        {
                            MaterialId = materialId,
                            Number = number,
                            UnitId = unitId
                        };
                        await CartRepository.InsertAsync(materialCart);
                    }
                }
                //删除订单
                await FlowSheetManager.DeleteAsync(flowSheet);
            }
            else if (action == "verify")
            {
                flowSheet.OrderStatus = "待发货";
            }
            else if (action == "cancel")
            {
                flowSheet.OrderStatus = "已取消";
            }
            else if (action == "send")
            {
                flowSheet.OrderStatus = "已发货";
                var totalFee = sheetHeader["totalFee"].ToObjectWithDefault<decimal>();
                //更改往来单位金额
                await UnitManager.ChangeFee(unitId, null, -totalFee, flowSheet);
                var materialSellIds = new List<int>();
                var toOutMaterials = new Dictionary<int, int>();
                var outStoreId = sheetHeader["storeId"].ToObjectWithDefault<int?>();
                if (outStoreId == null)
                {
                    throw new UserFriendlyException("请选择发货仓库");
                }
                
                foreach (var sheetItem in sheetData["body"])
                {
                    var materialId = sheetItem["materialId"].ToObjectWithDefault<int>();//商品Id
                    var number = sheetItem["number"].ToObjectWithDefault<int>();//出货数量
                    var materialSell = await MaterialSellManager.GetAll()
                        .Where(o => o.FlowSheetId == flowSheet.Id && o.MaterialId == materialId).FirstOrDefaultAsync();
                    if (materialSell != null)
                    {
                        materialSell.OutNumber = materialSell.SellNumber;//设置销售记录的出货数量等于订货数量
                    }
                    //建立销售出库记录
                    var materialSellOut = new MaterialSellOut()
                    {
                        UnitId = unitId,
                        FlowSheetId = flowSheet.Id,
                        MaterialId = materialId,
                        OutNumber = number,
                        Price = sheetItem["price"].ToObjectWithDefault<decimal>(),
                        Discount = sheetItem["discount"].ToObjectWithDefault<decimal>()
                    };
                    await MaterialSellOutManager.InsertAsync(materialSellOut);
                    //检测库存是否足够
                    if (!StoreMaterialManager.IsSatisfied(materialId, outStoreId.Value, unitId, number, out var message))
                    {
                        throw new UserFriendlyException(message);
                    }
                    //库存变化
                    await StoreMaterialManager.CountMaterial(outStoreId.Value, materialId, -number, flowSheet);

                }
                
            }
            else if (action == "back")
            {
                var totalFee = sheetHeader["totalFee"].ToObjectWithDefault<decimal>();
                flowSheet.OrderStatus = "已退货";
                //更改往来单位金额
                await UnitManager.ChangeFee(unitId, null, totalFee, flowSheet);
                var materialSellIds = new List<int>();
                var toOutMaterials = new Dictionary<int, int>();
                var backStoreId = sheetHeader["backStoreId"].ToObjectWithDefault<int?>();
                if (backStoreId == null)
                {
                    throw new UserFriendlyException("请选择退入仓库");
                }
                
                foreach (var sheetItem in sheetData["body"])
                {
                    var materialId = sheetItem["materialId"].ToObjectWithDefault<int>();//商品Id
                    var number = sheetItem["number"].ToObjectWithDefault<int>();//出货数量
                    //库存变化
                    await StoreMaterialManager.CountMaterial(backStoreId.Value, materialId, number, flowSheet);
                    //产生退货数据
                    var materialSellBack = new MaterialSellBack()
                    {
                        UnitId = unitId,
                        MaterialId = materialId,
                        BackNumber = number,
                        FlowSheetId = flowSheet.Id,
                        Discount = sheetItem["discount"].ToObjectWithDefault<decimal>(),
                        Price = sheetItem["price"].ToObjectWithDefault<decimal>(),
                    };
                    await MaterialSellBackManager.InsertAsync(materialSellBack);
                }
                
            }
        }
    }
}
