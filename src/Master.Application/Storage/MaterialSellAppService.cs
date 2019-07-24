using Abp.Authorization;
using Abp.Domain.Repositories;
using Master.EntityFrameworkCore;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    [AbpAuthorize]
    public class MaterialSellAppService:MasterAppServiceBase<MaterialSell,int>
    {
        /// <summary>
        /// 同步购物车
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="materialNature"></param>
        /// <param name="materialCartUpdateDtos"></param>
        /// <returns></returns>
        public virtual async Task SyncCart(int unitId,int materialNature,IEnumerable<MaterialCartUpdateDto> materialCartUpdateDtos)
        {
            var cartRepository = Resolve<IRepository<MaterialSellCart, int>>();
            //移除原有购物车
            await cartRepository.DeleteAsync(o => o.UnitId == unitId && o.Material.MaterialNature == (MaterialNature)materialNature && o.CreatorUserId == AbpSession.UserId);
            //加入新数据
            foreach(var materialCartDto in materialCartUpdateDtos)
            {
                var materialSellCart = new MaterialSellCart()
                {
                    UnitId = unitId,
                    MaterialId = materialCartDto.Id,
                    Number = materialCartDto.Number
                };
                await cartRepository.InsertAsync(materialSellCart);
            }
        }

        /// <summary>
        /// 获取当前用户某代理的购物车
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="materialNature"></param>
        /// <returns></returns>
        public virtual async Task<object> GetCartInfo(int unitId,MaterialNature materialNature)
        {
            var cartRepository = Resolve<IRepository<MaterialSellCart, int>>();
            return await cartRepository.GetAll()
                .Include(o=>o.Material)
                .Where(o => o.UnitId == unitId && o.Material.MaterialNature == materialNature && o.CreatorUserId==AbpSession.UserId)
                .Select(o => new
                {
                    Id=o.MaterialId,
                    o.Material.Specification,
                    o.Material.Location,
                    o.Material.Price,
                    Name=o.Material.Name,
                    MeasureMentUnit=o.Material.MeasureMentUnit,
                    Number=o.Number
                })
                .ToListAsync();
        }

        /// <summary>
        /// 代理商所有待出库的商品
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual async Task<object> GetUnitSellMaterialForOut(int unitId)
        {
            var result = new List<object>();
            var sellMaterials= await Manager.GetAll().Include(o => o.FlowSheet).Include(o => o.Material)
                .Where(o => o.Material.MaterialNature == MaterialNature.票券)
                .Where(o=>o.UnitId==unitId)
                .Where(o=>MasterDbContext.GetJsonValueString(o.FlowSheet.Property,"$.OrderStatus")!="待审核")//不显示待审核的商品
                .Where(o=>o.SellNumber>o.OutNumber)//不显示已完全出库的商品
                .ToListAsync();

            var materialManager = Resolve<MaterialManager>();
            foreach(var sellMaterial in sellMaterials)
            {
                //获取对应代理商的折扣
                
                result.Add(new
                {
                    sellMaterial.Id,
                    sellMaterial.MaterialId,
                    sellMaterial.Material.Name,
                    sellMaterial.Material.Specification,
                    sellMaterial.Material.Price,
                    sellMaterial.FlowSheet.SheetSN,
                    SheetDate=sellMaterial.FlowSheet.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                    sellMaterial.SellNumber,
                    sellMaterial.OutNumber,
                    discount= await materialManager.GetMaterialUnitDiscount(sellMaterial.Material,unitId)
                });
            }

            return result;
        }
        /// <summary>
        /// 获取代理商可退货商品
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public virtual async Task<object> GetUnitSellMaterialForBack(int unitId, DateTime startDate)
        {
            var materialManager = Resolve<MaterialManager>();
            var sellMaterials = (await Manager.GetAll().Include(o => o.FlowSheet).Include(o => o.Material)
                .Where(new MaterialSellSpecification(unitId,startDate))
                .Where(o => MasterDbContext.GetJsonValueString(o.FlowSheet.Property, "$.OrderStatus") != "待审核")//不显示待审核的商品
                .Where(o => o.SellNumber > o.BackNumber)//
                .GroupBy(o => o.Material)
                .ToListAsync())
                .Select(o => new
                {
                    o.Key.Id,
                    o.Key.Name,
                    o.Key.Specification,
                    o.Key.Price,
                    SellNumber = o.Sum(b => b.SellNumber),
                    BackNumber = o.Sum(b => b.BackNumber),
                    CanBackNumber = o.Sum(b => b.CanBackNumber),
                    discount = materialManager.GetMaterialUnitDiscount(o.Key, unitId).Result
                })
                ;
            return sellMaterials;
            //var materialManager = Resolve<MaterialManager>();
            //foreach (var sellMaterial in sellMaterials)
            //{
            //    //获取对应代理商的折扣

            //    result.Add(new
            //    {
            //        sellMaterial.Id,
            //        sellMaterial.MaterialId,
            //        sellMaterial.Material.Name,
            //        sellMaterial.Material.Specification,
            //        sellMaterial.Material.Price,
            //        sellMaterial.FlowSheet.SheetSN,
            //        SheetDate = sellMaterial.FlowSheet.CreationTime.ToString("yyyy-MM-dd HH:mm"),
            //        sellMaterial.SellNumber,
            //        sellMaterial.BackNumber,
            //        discount = await materialManager.GetMaterialUnitDiscount(sellMaterial.Material, unitId)
            //    });
            //}

            //return result;
        }
    }
}
