using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Entity;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Application
{
    /// <summary>
    /// 日志
    /// </summary>
    public class SystemLog : FullAuditedEntity<int>, IMayHaveTenant, IHaveProperty
    {
        /// <summary>
        /// 日志名称，如"实体变更记录"
        /// </summary>
        public string LogName { get; set; }
        /// <summary>
        /// 日志分组:如记录的实体类名：Master.MultiTenancy.Tenat
        /// </summary>
        public string LogGroup { get; set; }
        /// <summary>
        /// 对应实体标识：如实体Id
        /// </summary>
        public string LogEntityIdentifier { get; set; }        
        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }
        public int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        [Column(TypeName = "json")]
        public JsonObject<IDictionary<string, object>> Property { get; set; }
    }
}
