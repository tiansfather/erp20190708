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
    /// 票劵订货单
    /// </summary>
    public class SDDFlowHandler : FlowHandlerBase
    {
        
        public MaterialManager MaterialManager { get; set; }
        public MaterialSellManager MaterialSellManager { get; set; }
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
            flowSheet.SetPropertyValue("OrderStatus",CurrentUser.IsCenterUser?"待出库": "待审核");
            flowSheet.SetPropertyValue("OrderType", CurrentUser.IsCenterUser?"中心代为下单": "代理商自助下单");
            //清空对应代理商的票劵购物车
            await CartRepository.DeleteAsync(o => o.CreatorUserId == AbpSession.UserId && o.UnitId == unitId && o.Material.MaterialNature == MaterialNature.票券);

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
            //if (flowSheet.GetPropertyValue<string>("OrderStatus") == "待审核")
            if (true)
                {
                btns.Add(new ModuleButton()
                {
                    ButtonKey = "backToCart",
                    ButtonName = "放回购物车修改",
                    ConfirmMsg="确认将订单放回购物车?此订单将失效"
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
        }
    }
}
