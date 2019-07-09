using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Dto
{
    /// <summary>
    /// 字段分组查询Dto
    /// </summary>
    public class RequestGroupFieldPageDto:RequestPageDto
    {
        /// <summary>
        /// 要查询的字段名如“Name"
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 提交过来的关键字
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// 重写每页条数，默认返回50
        /// </summary>
        public override int Limit { get; set; } = 50;
    }
}
