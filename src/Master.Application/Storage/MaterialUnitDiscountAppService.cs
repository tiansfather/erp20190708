using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Master.Dto;
using Master.Units;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{
    [AbpAuthorize]
    public class MaterialUnitDiscountAppService : MaterialAppService
    {
        protected override string ModuleKey()
        {
            return nameof(MaterialUnitDiscount);
        }

        protected override async Task<IQueryable<Material>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request))
                .Where(o=>o.MaterialType.Nature==0);//只查询对外的分类
        }
       
        /// <summary>
        /// 获取物料的代理商折扣明细
        /// </summary>
        /// <param name="unitMaterialDiscountSearchDto"></param>
        /// <returns></returns>
        public virtual async Task<object> GetUnitMaterialDiscountInfo(UnitMaterialDiscountSearchDto unitMaterialDiscountSearchDto)
        {
            var result = new List<object>();
            var units = await Resolve<UnitManager>().GetAllList();
            if (unitMaterialDiscountSearchDto.UnitId.HasValue)
            {
                units = units.Where(o => o.Id == unitMaterialDiscountSearchDto.UnitId.Value).ToList();
            }
            if (unitMaterialDiscountSearchDto.UnitRanks.Length > 0)
            {
                units = units.Where(o => unitMaterialDiscountSearchDto.UnitRanks.Contains(o.UnitRank)).ToList();
            }
            var disCountRepository = Resolve<IRepository<UnitMaterialDiscount, int>>();
            foreach(var unit in units)
            {
                var unitDiscount = await disCountRepository.GetAll().Where(o => o.MaterialId == unitMaterialDiscountSearchDto.MaterialId && o.UnitId == unit.Id).SingleOrDefaultAsync();
                var obj = new
                {
                    UnitId=unit.Id,
                    unit.UnitName,
                    unit.UnitRank,
                    UnitDiscount=(unitDiscount!=null?Convert.ToInt32(unitDiscount.UnitDiscount):0),
                    UnitSellMode= (unitDiscount != null ? Convert.ToInt32(unitDiscount.UnitSellMode) : 0).ToString(),
                };

                result.Add(obj);
            }

            return result;
        }

        public virtual async Task SetMaterialUnitDiscountInfo(int materialId,IEnumerable<UnitMaterialDiscountSubmitDto> unitMaterialDiscountSubmitDtos)
        {
            var disCountRepository = Resolve<IRepository<UnitMaterialDiscount, int>>();
            foreach(var unitMaterialDiscountSubmitDto in unitMaterialDiscountSubmitDtos)
            {
                var unitDiscount = await disCountRepository.GetAll().Where(o => o.MaterialId == materialId && o.UnitId == unitMaterialDiscountSubmitDto.UnitId).SingleOrDefaultAsync();
                if (unitDiscount != null)
                {
                    unitMaterialDiscountSubmitDto.MapTo(unitDiscount);
                    //unitDiscount.UnitDiscount = unitMaterialDiscountSubmitDto.UnitDiscount;
                    //unitDiscount.UnitSellMode = unitMaterialDiscountSubmitDto.UnitSellMode;
                }
                else
                {
                    unitDiscount = unitMaterialDiscountSubmitDto.MapTo<UnitMaterialDiscount>();
                    unitDiscount.MaterialId = materialId;
                    await disCountRepository.InsertAsync(unitDiscount);
                }
            }
            
        }
    }
    [AutoMap(typeof(UnitMaterialDiscount))]
    public class UnitMaterialDiscountSubmitDto
    {
        public int UnitId { get; set; }
        public UnitDiscount UnitDiscount { get; set; }
        public UnitSellMode UnitSellMode { get; set; }
    }
    public class UnitMaterialDiscountSearchDto
    {
        public int MaterialId { get; set; }
        public int? UnitId { get; set; }
        public int[] UnitRanks { get; set; }
    }
}
