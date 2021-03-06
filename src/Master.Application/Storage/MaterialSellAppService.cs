﻿using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Master.Dto;
using Master.Entity;
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
        public BaseTreeManager BaseTreeManager { get; set; }
        #region 分页
        protected override async Task<IQueryable<MaterialSell>> GetQueryable(RequestPageDto request)
        {
            var user = await GetCurrentUserAsync();
            return (await base.GetQueryable(request))
                .Include(o=>o.FlowSheet)
                .Include(o=>o.Material).ThenInclude(o=>o.MaterialType)
                .Where(o => o.FlowSheet.SheetNature == WorkFlow.SheetNature.正单)
                .Where(o=>o.FlowSheet.OrderStatus==null ||(o.FlowSheet.OrderStatus!="待审核" && o.FlowSheet.OrderStatus != "已退货" && o.FlowSheet.OrderStatus != "已取消"))
                 .WhereIf(user.UnitId.HasValue, o => o.FlowSheet.UnitId == user.UnitId)//代理商登录只看到自己;
                ;
        }
        protected override async Task<IQueryable<MaterialSell>> BuildKeywordQueryAsync(string keyword, IQueryable<MaterialSell> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o => o.Material.MaterialType.DisplayName.Contains(keyword) || o.Material.Name.Contains(keyword) || o.FlowSheet.SheetSN.Contains(keyword));
        }
        protected override async Task<IQueryable<MaterialSell>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<MaterialSell> query)
        {
            
            if (searchKeys.ContainsKey("MaterialTypeId"))
            {
                if (int.TryParse(searchKeys["MaterialTypeId"], out var materialTypeId))
                {
                    if (materialTypeId == -1)
                    {
                        query = query.Where(o => o.Material.MaterialTypeId == null);
                    }
                    else
                    {
                        var materialType = await BaseTreeManager.GetByIdFromCacheAsync(materialTypeId);
                        //所有子类的id集合
                        var childIds = (await BaseTreeManager.FindChildrenAsync(materialTypeId, materialType.Discriminator, true)).Select(o => o.Id);
                        query = query.Where(o => o.Material.MaterialTypeId == materialTypeId || o.Material.MaterialTypeId != null && childIds.Contains(o.Material.MaterialTypeId.Value));
                    }

                }
            }
            if (searchKeys.ContainsKey("startDate"))
            {
                query = query.Where(o => o.CreationTime > DateTime.Parse(searchKeys["startDate"]));
            }
            if (searchKeys.ContainsKey("endDate"))
            {
                query = query.Where(o => o.CreationTime < DateTime.Parse(searchKeys["endDate"]));
            }
            return query;
        }

        protected override object PageResultConverter(MaterialSell entity)
        {
            return new
            {
                entity.FlowSheet?.SheetSN,
                entity.Material?.Name,
                MaterialTypeName=entity.Material?.MaterialType?.DisplayName,
                UnitName=entity.Unit?.UnitName,
                entity.Material?.Specification,
                MaterialNature=entity.Material?.MaterialNature.ToString(),
                entity.Material?.Price,
                entity.Material?.MeasureMentUnit,
                entity.SellNumber,
                OutNumber=entity.OutNumber-entity.BackNumber,
                CreationTime=entity.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                Receiver=entity.FlowSheet?.GetPropertyValue<string>("Receiver"),
                DeliverDate=entity.FlowSheet?.GetPropertyValue<DateTime?>("DeliverDate")?.ToString("yyyy-MM-dd"),
                ReceiverMobile=entity.FlowSheet?.GetPropertyValue<string>("ReceiverMobile"),
                ReceiverAddress= entity.FlowSheet?.GetPropertyValue<string>("ReceiverAddress"),
            };
        }
        #endregion

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
            var result = new List<object>();
            var cartRepository = Resolve<IRepository<MaterialSellCart, int>>();
            var materialManager = Resolve<MaterialManager>();
            var storeMaterialManager = Resolve<StoreMaterialManager>();
            var sellMaterials = await cartRepository.GetAll()
                .Include(o=>o.Material)
                .Where(o => o.UnitId == unitId && o.Material.MaterialNature == materialNature && o.CreatorUserId==AbpSession.UserId)
                .Select(o => new
                {
                    Id=o.MaterialId,
                    o.Material,
                    o.Material.Specification,
                    o.Material.Location,
                    o.Material.Price,
                    o.Material.Remarks,
                    Name=o.Material.Name,
                    MeasureMentUnit=o.Material.MeasureMentUnit,
                    Number=o.Number
                })
                .ToListAsync();
            
            foreach (var sellMaterial in sellMaterials)
            {
                //获取对应代理商的折扣
                var sellMode = await materialManager.GetMaterialUnitSellMode(sellMaterial.Material, unitId);
                var storeNumber = await storeMaterialManager.GetMaterialNumber(sellMaterial.Id);
                //是否可以销售，售完为止的且数量大于库存的为false
                var canSell = sellMode == UnitSellMode.始终销售 || sellMode == UnitSellMode.售完为止 && storeNumber >= sellMaterial.Number;
                result.Add(new
                {
                    sellMaterial.Id,
                    MaterialId=sellMaterial.Material.Id,
                    sellMaterial.Name,
                    sellMaterial.Specification,
                    sellMaterial.Price,
                    sellMaterial.MeasureMentUnit,
                    sellMaterial.Number,
                    sellMaterial.Location,
                    sellMaterial.Remarks,
                    discount = await materialManager.GetMaterialUnitDiscount(sellMaterial.Material, unitId),
                    storeNumber,
                    canSell
                });
            }

            return result;
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
                .Where(o=>o.FlowSheet.OrderStatus!="待审核")//不显示待审核的商品
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
                    sellMaterial.Material.MeasureMentUnit,
                    sellMaterial.Material.Price,
                    sellMaterial.FlowSheet.SheetSN,
                    sellMaterial.FlowSheetId,
                    SheetDate=sellMaterial.FlowSheet.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                    sellMaterial.SellNumber,
                    sellMaterial.OutNumber,
                    sellMaterial.Discount,
                    //discount= await materialManager.GetMaterialUnitDiscount(sellMaterial.Material,unitId),
                    sellMode=await materialManager.GetMaterialUnitSellMode(sellMaterial.Material,unitId),
                    CreationTime=sellMaterial.CreationTime.ToString("yyyy-MM-dd"),
                    Number=sellMaterial.SellNumber-sellMaterial.OutNumber,//默认发货数量等于订货数量-已发数量
                    storeNumber=0
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
                .Where(o => o.FlowSheet.OrderStatus != "待审核" && o.FlowSheet.OrderStatus!= "待出库")//不显示待审核待出库的商品
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

        public virtual async Task<object> CheckSellMaterialInfos(IEnumerable<CheckSellMaterialInfoDto> checkSellMaterialInfoDtos,int unitId)
        {
            var runoutMaterialIds = new List<int>();//订货数量>库存
            var limitedMaterialIds = new List<int>();//订货数量+未出库数量>库存
            foreach(var checkSellMaterialInfoDto in checkSellMaterialInfoDtos)
            {
                var limitType = await LimitSellMaterialInfo(checkSellMaterialInfoDto, unitId);
                if (limitType == 1)
                {
                    runoutMaterialIds.Add(checkSellMaterialInfoDto.Id);
                }
                else if (limitType == 2)
                {
                    limitedMaterialIds.Add(checkSellMaterialInfoDto.Id);
                }
            }
            if (runoutMaterialIds.Count > 0)
            {
                var msg = string.Join(',', (await Resolve<MaterialManager>().GetListByIdsAsync(runoutMaterialIds)).Select(o => o.Name));
                return new
                {
                    msg,
                    code = 1
                };
            }
            else if (limitedMaterialIds.Count > 0)
            {
                var msg = string.Join(',', (await Resolve<MaterialManager>().GetListByIdsAsync(limitedMaterialIds)).Select(o => o.Name));
                return new
                {
                    msg,
                    code=2
                };
            }
            else
            {
                return new { code=0};
            }
        }
        /// <summary>
        /// 0:正常,1:出货数量超过库存,2:出货数量+未出库>超过库存
        /// </summary>
        /// <param name="checkSellMaterialInfoDto"></param>
        /// <returns></returns>
        private async Task<int> LimitSellMaterialInfo(CheckSellMaterialInfoDto checkSellMaterialInfoDto,int unitId)
        {
            var user = await GetCurrentUserAsync();
            var storeMaterialManager = Resolve<StoreMaterialManager>();
            var materialManager = Resolve<MaterialManager>();
            var material = await materialManager.GetByIdAsync(checkSellMaterialInfoDto.Id);
            var sellMode = await materialManager.GetMaterialUnitSellMode(material, unitId);
            if (sellMode == UnitSellMode.停止销售)
            {
                return 1;
            }else if(sellMode == UnitSellMode.始终销售)
            {
                return 0;
            }
            else
            {
                //库存数量，代理商登录就是默认仓库的库存数量
                var storeNumber = await storeMaterialManager.GetMaterialNumber(checkSellMaterialInfoDto.Id);
                //获取已下单但未出库的数量
                var toOutNumber = await Manager.GetAll()
                    .Where(o => o.MaterialId == checkSellMaterialInfoDto.Id)
                    .WhereIf(!user.IsCenterUser, o => MasterDbContext.GetJsonValueString(o.FlowSheet.Property, "$.OrderType") == "代理商自助下单")
                    .SumAsync(o => o.SellNumber - o.OutNumber);
                if (checkSellMaterialInfoDto.Number > storeNumber)
                {
                    return 1;
                }
                else if(checkSellMaterialInfoDto.Number + toOutNumber > storeNumber)
                {
                    return 2;
                }
                return 0;
            }
            
        }
    }
}
