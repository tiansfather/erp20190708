using Abp.Domain.Entities;
using Master.Entity;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    [InterModule("仓库档案")]
    public class Store : BaseFullEntityWithTenant,IPassivable
    {
        /// <summary>
        /// 仓库编码
        /// </summary>
        [InterColumn(ColumnName = "仓库编码",Sort =1)]
        public string StoreCode { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        [InterColumn(ColumnName = "仓库名称", VerifyRules = "required",Sort =2)]
        public string Name { get; set; }
        /// <summary>
        /// 仓库地址
        /// </summary>
        [InterColumn(ColumnName = "仓库地址",Sort =3)]
        public string Address { get; set; }
        [InterColumn(ColumnName = "默认", DefaultValue = "false",IsShownInMultiEdit =false, ColumnType = Module.ColumnTypes.Switch, Templet = "{{#if(d.isDefault){}}<span class=\"layui-badge layui-bg-green\">默认</span>{{#}else{}}{{#}}}", Sort = 4)]
        public bool IsDefault { get; set; }
        [InterColumn(ColumnName ="状态",DefaultValue ="true",ColumnType =Module.ColumnTypes.Switch,Templet = "{{#if(d.isActive){}}<span class=\"layui-badge layui-bg-green\">有效</span>{{#}else{}}<span class=\"layui-badge layui-bg-gray\">无效</span>{{#}}}",Sort =5)]
        public bool IsActive { get; set; }
    }
}
