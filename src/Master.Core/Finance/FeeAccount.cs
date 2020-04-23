using Abp.Domain.Entities;
using Master.Entity;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Finance
{
    [InterModule("资金账户档案",GenerateOperateColumn =true)]
    public class FeeAccount : BaseFullEntityWithTenant, IPassivable
    {
        public const string StaticAccountName1 = "现金账户";
        public const string StaticAccountName2 = "支票账户";
        public const string StaticAccountName3 = "过账账户";
        [InterColumn(ColumnName = "账户名称", VerifyRules = "required",Sort =1)]
        public string Name { get; set; }
        [InterColumn(ColumnName = "状态",DefaultValue ="true", ColumnType = Module.ColumnTypes.Switch, Templet = "{{#if(d.isActive){}}<span class=\"layui-badge layui-bg-green\">有效</span>{{#}else{}}<span class=\"layui-badge layui-bg-gray\">无效</span>{{#}}}",Sort =2)]
        public bool IsActive { get; set; }
        [InterColumn(ColumnName = "期初余额", ColumnType = Module.ColumnTypes.Number,DisplayFormat ="0.00",DefaultValue ="0",VerifyRules ="number",Sort =3)]
        [Column(TypeName = "decimal(20,2)")]
        public decimal StartFee { get; set; }
        /// <summary>
        /// 当前余额
        /// </summary>
        [InterColumn(ColumnName = "当前余额", ColumnType = Module.ColumnTypes.Number, DisplayFormat = "0.00",IsShownInAdd =false,IsShownInEdit =false,Sort =4, Templet = "{{(Math.round((parseFloat(d.startFee)+parseFloat(d.fee))*100)/100).toFixed(2)}}")]
        [Column(TypeName = "decimal(20,2)")]
        public decimal Fee { get; set; }
    }
}
