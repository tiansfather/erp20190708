using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Resources
{
    /// <summary>
    /// 标记
    /// </summary>
    public class FlagInfo
    {
        /// <summary>
        /// 标记名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string BriefName { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 标记用途，目前只在Tenant标记中使用，用于区分是加工点的标签还是管理标签
        /// </summary>
        public string Identifier { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        /// <summary>
        /// 是否系统标记：即由Host创建的标记
        /// </summary>
        public bool System { get; set; }
    }
}
