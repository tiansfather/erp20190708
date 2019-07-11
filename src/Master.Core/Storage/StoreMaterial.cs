using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Master.Entity;
using Master.Module.Attributes;
using Master.Projects;
using Master.Units;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Storage
{
    /// <summary>
    /// 库存信息
    /// </summary>
    [InterModule("库存信息")]
    public class StoreMaterial : BaseFullEntityWithTenant, IHaveStatus
    {
        /// <summary>
        /// 存货
        /// </summary>
        [InterColumn(ColumnName = "物资编码", DisplayPath = "Material.Code", Templet = "{{d.materialId_display}}" ,Sort = 0)]
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }                
        /// <summary>
        /// 仓库
        /// </summary>
        [InterColumn(ColumnName = "仓库", ValuePath = "Store.Name", Sort = 7)]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }        

        /// <summary>
        /// 库存数量
        /// </summary>
        [InterColumn(ColumnName = "库存数量",ColumnType =Module.ColumnTypes.Number, Sort = 10,DisplayFormat ="0.00")]
        public decimal Number { get; set; }
        
        public string Status { get; set; }

        /// <summary>
        /// 库存变动明细记录值
        /// </summary>
        [NotMapped]
        public virtual StoreMaterialHistory StoreMaterialHistory { get;set;}
        /// <summary>
        /// 产生库存明细账实体
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public virtual void BuildStoreMaterialHistory(string ChangeType,decimal Number,int? FlowSheetId)
        { 
 

                this.StoreMaterialHistory = new StoreMaterialHistory()
                { 
                    ChangeType = ChangeType,
                    Number = Number,
                    FlowSheetId= FlowSheetId,
                };
                
           
            
        }
    }
}
