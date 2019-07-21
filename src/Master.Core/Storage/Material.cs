using System;
using System.Collections.Generic;
using System.Text;
using Master.Entity;
using Master.Module;
using Master.Module.Attributes;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Specifications;
using System.Linq.Expressions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Master.Storage
{
    /// <summary>
    /// 物料
    /// </summary>
    [InterModule("商品档案")]
    public class Material : BaseFullEntityWithTenant, IHaveStatus, IPassivable
    {
        /// <summary>
        /// 物料名称
        /// </summary>
        [InterColumn(ColumnName = "品名", VerifyRules = "required", Sort = 1)]
        public string Name { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        [InterColumn(ColumnName = "规格", Sort = 2)]
        public string Specification { get; set; }
        
        [InterColumn(ColumnName = "分类", Renderer = "lay-materialtypechoose", DisplayPath = "MaterialType.DisplayName", Templet = "{{d.materialTypeId_display||''}}", Sort = 3)]
        public int? MaterialTypeId { get; set; }
        public virtual BaseTree MaterialType { get; set; }
        /// <summary>
        /// 性质
        /// </summary>
        [InterColumn(ColumnName = "性质", ColumnType = ColumnTypes.Select,DefaultValue = "票券", DictionaryName = "Master.Storage.MaterialNature", Sort = 4, Templet = "{{d.materialNature_display}}")]
        public virtual MaterialNature MaterialNature { get; set; } = MaterialNature.票券;
        /// <summary>
        /// 散装配置
        /// </summary>
        [InterColumn(ColumnName = "散装配置", ColumnType = ColumnTypes.Select, DefaultValue = "单品", DictionaryName = "Master.Storage.MaterialDIYType", Sort = 5, Templet = "{{d.materialDIYType_display}}",IsShownInList =false)]
        public virtual MaterialDIYType MaterialDIYType { get; set; } = MaterialDIYType.单品;
        /// <summary>
        /// 计量单位
        /// </summary>
        [InterColumn(ColumnName = "计量单位", VerifyRules = "required", Sort = 6)]
        public string MeasureMentUnit { get; set; }
        [InterColumn(ColumnName = "市场单价(元)", ColumnType = ColumnTypes.Number, VerifyRules = "number", DefaultValue = "0", DisplayFormat = "0.00", Sort = 7)]
        public decimal Price { get; set; }
        [InterColumn(ColumnName = "库存下限", ColumnType = ColumnTypes.Number, VerifyRules = "numberOrempty",IsShownInList =false, DefaultValue = "0", DisplayFormat = "0", Sort = 8)]
        public decimal? LimitDown { get; set; }
        public string Status { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [InterColumn(ColumnName = "状态", ColumnType = Module.ColumnTypes.Switch,DefaultValue ="true", Templet = "{{#if(d.isActive){}}<span class=\"layui-badge layui-bg-green\">上架</span>{{#}else{}}<span class=\"layui-badge layui-bg-gray\">下架</span>{{#}}}", Sort = 9)]
        public bool IsActive { get; set; } = true;
        [InterColumn(ColumnName ="默认进货折扣",ColumnType =ColumnTypes.Number,Sort =10, DisplayFormat = "0.00", IsShownInList = false,DefaultValue ="1",VerifyRules ="required|number")]
        public decimal? DefaultBuyDiscount { get; set; }
        [InterColumn(ColumnName = "默认出货折扣", ColumnType = ColumnTypes.Number, Sort = 11, DisplayFormat = "0.00", IsShownInList = false, DefaultValue = "1", VerifyRules = "required|number")]
        public decimal? DefaultSellDiscount { get; set; }
        [InterColumn(ColumnName = "出货折扣1", ColumnType = ColumnTypes.Number, Sort =12, DisplayFormat = "0.00", IsShownInList = false, DefaultValue = "1", VerifyRules = "required|number")]
        public decimal? SellDiscount1 { get; set; }
        [InterColumn(ColumnName = "出货折扣2", ColumnType = ColumnTypes.Number, Sort = 13, DisplayFormat = "0.00", IsShownInList = false, DefaultValue = "1", VerifyRules = "required|number")]
        public decimal? SellDiscount2 { get; set; }
        [InterColumn(ColumnName = "出货折扣3", ColumnType = ColumnTypes.Number, Sort = 14, DisplayFormat = "0.00", IsShownInList = false, DefaultValue = "1", VerifyRules = "required|number")]
        public decimal? SellDiscount3 { get; set; }
        [InterColumn(ColumnName ="适用区域",IsShownInList =false,Sort =15)]
        public string Location { get; set; }
        [InterColumn(ColumnName ="备注",Sort =16,IsShownInList =false)]
        public override string Remarks { get; set; }

        public virtual bool IsDiyed { get; set; }
        /// <summary>
        /// 组装信息
        /// </summary>
        [NotMapped]
        public List<DIYInfo> DIYInfo
        {
            get
            {
                var diyInfos=this.GetData<List<DIYInfo>>("DIYInfo");
                return diyInfos ?? new List<DIYInfo>();
            }
            set
            {
                if (value.Count > 0)
                {
                    this.IsDiyed = true;
                }
                this.SetData("DIYInfo", value);
            }
        }
    }

    public enum MaterialNature
    {
        票券,
        实物
    }

    public enum MaterialDIYType
    {
        单品,
        组装
    }

    public class MaterialSellUnitSpecification : Specification<Material>
    {
        private int _unitId;
        public MaterialSellUnitSpecification(int unitId)
        {
            _unitId = unitId;
        }
        public override Expression<Func<Material, bool>> ToExpression()
        {
            using(var scope = IocManager.Instance.CreateScope())
            {
                var repository = scope.Resolve<IRepository<UnitMaterialDiscount, int>>();
                var storeMaterialManager = scope.Resolve<StoreMaterialManager>();

                return o => repository.Count(d => d.UnitId == _unitId && d.MaterialId == o.Id) == 0 //并未设置代理商销售方式的默认始终销售
                ||repository.Count(d=> d.UnitId == _unitId && d.MaterialId == o.Id && d.UnitSellMode==UnitSellMode.始终销售)>0 //始终销售的展示
                    || repository.Count(d => d.UnitId == _unitId && d.MaterialId == o.Id && d.UnitSellMode == UnitSellMode.售完为止 && storeMaterialManager.GetAll().Where(s => s.MaterialId == o.Id).Sum(s=>s.Number) > 0)>0;//售完为止且库存数量大于0的显示
            }
        }
    }
}
