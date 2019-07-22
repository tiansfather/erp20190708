using Abp.Authorization;
using Abp.Domain.Repositories;
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
    }
}
