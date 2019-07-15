using Master.Entity;
using Master.Module.Attributes;
using Master.Projects;
using Master.Units;
using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    /// <summary>
    /// 库存变动明细
    /// </summary>
    [InterModule("库存变动明细",GenerateDefaultButtons =false,GenerateDefaultColumns =false)]
    public class StoreMaterialHistory : BaseFullEntityWithTenant
    {
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }        
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        
        [InterColumn(ColumnName = "品名",Sort =1)]
        public string MaterialName { get; set; }
        [InterColumn(ColumnName = "规格",Sort =2)]
        public string Specification { get; set; }
        public string Brand { get; set; }        
        public string UnitName { get; set; }
        [InterColumn(ColumnName = "仓库",Sort =7)]
        public string StoreName { get; set; }        

        [InterColumn(ColumnName = "单位",Sort =9)]
        public string MeasureMentUnitName { get; set; }
        [InterColumn(ColumnName = "变动类型",Sort =10)]
        public string ChangeType { get; set; }
        /// <summary>
        /// 变动数量
        /// </summary>
        [InterColumn(ColumnName = "变动数量",Sort =11)]
        public decimal Number { get; set; }
        [InterColumn(ColumnName = "单据",DisplayPath ="FlowSheet.SheetSN" , Templet = "{{#if (d.flowSheetId){}}<a dataid=\"{{d.flowSheetId}}\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"单据\" params=\"{&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/FlowSheet/SheetView\" onclick=\"func.callModuleButtonEvent()\">{{d.flowSheetId_display||'未录入单号'}}</a>{{#}else{}}{{#}}}", Sort =12)]
        public int? FlowSheetId { get; set; } 


        public virtual FlowSheet FlowSheet { get; set; }
    }
}
