using Abp.Authorization;
using Master.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    [AbpAuthorize]
    public class MaterialDIYAppService : MaterialAppService
    {
        protected override string ModuleKey()
        {
            return nameof(MaterialDIY);
        }

        protected override async Task<IQueryable<Material>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Where(o=>o.MaterialDIYType==MaterialDIYType.组装 && o.MaterialNature==MaterialNature.实物);
        }
        protected override async Task<IQueryable<Material>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<Material> query)
        {
            query=await base.BuildSearchQueryAsync(searchKeys, query);

            if (searchKeys.ContainsKey("materialDIYType"))
            {
                query = query.Where($"materialDIYType={searchKeys["materialDIYType"]}");
            }

            return query;
        }
        /// <summary>
        /// 获取某商品的组装信息
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public virtual async Task<object> GetMaterialDiyInfo(int materialId)
        {
            var material = await Manager.GetByIdFromCacheAsync(materialId);
            var diyInfo = material.DIYInfo;

            var diyMaterials = await Manager.GetListByIdsAsync(diyInfo.Select(o => o.MaterialId));

            return new
            {
                Name = material.Name,
                DiyInfos = diyMaterials.Select(o => new
                {
                    o.Name,
                    o.Specification,
                    o.MeasureMentUnit,
                    o.Price,
                    Number=diyInfo.First(d=>d.MaterialId==o.Id).Number
                })
            };
        }
        /// <summary>
        /// 设置组装信息
        /// </summary>
        /// <param name="materialDIYSubmitDto"></param>
        /// <returns></returns>
        public virtual async Task SetMaterialDiyInfo(MaterialDIYSubmitDto materialDIYSubmitDto)
        {
            var material = await Manager.GetByIdAsync(materialDIYSubmitDto.Id);
            material.DIYInfo = materialDIYSubmitDto.DIYInfos.ToList();
        }
    }
}
