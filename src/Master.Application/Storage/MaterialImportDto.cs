using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Master.Entity;
using Master.Imports;
using Master.Units;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Storage
{

    [AutoMap(typeof(Material))]
    public class MaterialImportDto : IImport
    {
        [DisplayName("品名")]
        [ImportField(Required = true)]
        public string Name { get; set; }
 
        [DisplayName("规格")]
        public string Specification { get; set; }

        [DisplayName("分类")]
        [ImportField(Required = true)]
        public string MaterialTypeName { get; set; }

        [DisplayName("计量单位")]
        [ImportField(Required = true)]
        public string MeasureMentUnit { get; set; }
        [DisplayName("性质")]
        [ImportField(Required = true,ColumnTypes =Module.ColumnTypes.Select,DictionaryName = "{\"票劵\":\"票劵\",\"实物\":\"实物\"}")]
        public string MaterialNatureName { get; set; }
        [DisplayName("散装配置")]
        [ImportField(Required = true, ColumnTypes = Module.ColumnTypes.Select, DictionaryName = "{\"单品\":\"单品\",\"组合\":\"组合\"}")]
        public string MaterialDIYTypeName { get; set; }

        [DisplayName("市场单价(元)")]
        [ImportField(ColumnTypes =Module.ColumnTypes.Number)]
        public decimal Price { get; set; } = 0;
        [DisplayName("库存下限")]
        [ImportField(ColumnTypes = Module.ColumnTypes.Number)]
        public decimal LimitDown { get; set; } = 0;

        [DisplayName("默认进货折扣")]
        [ImportField(ColumnTypes = Module.ColumnTypes.Number)]
        public decimal DefaultBuyDiscount { get; set; } = 1;
        [DisplayName("默认出货折扣")]
        [ImportField(ColumnTypes = Module.ColumnTypes.Number)]
        public decimal DefaultSellDiscount { get; set; } = 1;
        [DisplayName("出货折扣1")]
        [ImportField(ColumnTypes = Module.ColumnTypes.Number)]
        public decimal SellDiscount1 { get; set; } = 1;
        [DisplayName("出货折扣2")]
        [ImportField(ColumnTypes = Module.ColumnTypes.Number)]
        public decimal SellDiscount2 { get; set; } = 1;
        [DisplayName("出货折扣3")]
        [ImportField(ColumnTypes = Module.ColumnTypes.Number)]
        public decimal SellDiscount3 { get; set; } = 1;
        [DisplayName("适用区域")]
        public string Location { get; set; }

        public async Task Import(IDictionary<string, object> parameter)
        {

            using (var scope = IocManager.Instance.CreateScope())
            {
                var currentUnitOfWork = scope.Resolve<IUnitOfWorkManager>();
                var material = this.MapTo<Material>();
                var manager = scope.Resolve<MaterialManager>();
                var baseTreeManager = scope.Resolve<BaseTreeManager>();
                var outi = 0;

                #region 判断分类
                var MaterialTypeName = this.MaterialTypeName;
                if (!string.IsNullOrEmpty(MaterialTypeName)) {
                    var baseTreeMaterialType = await baseTreeManager.GetByName(MaterialTypeName, "MaterialType");
                    if (baseTreeMaterialType != null)
                    {
                    }
                    else {
                        throw new UserFriendlyException("分类{" + MaterialTypeName + "}不存在");
                    }

                    material.MaterialTypeId = baseTreeMaterialType.Id;
                }
                #endregion

                if (this.MaterialNatureName == "实物")
                {
                    material.MaterialNature = MaterialNature.实物;
                }
                if (this.MaterialDIYTypeName == "组装")
                {
                    material.MaterialDIYType = MaterialDIYType.组装;
                }

                await manager.InsertAsync(material);
                await currentUnitOfWork.Current.SaveChangesAsync();
            }
        }

    }
}

 