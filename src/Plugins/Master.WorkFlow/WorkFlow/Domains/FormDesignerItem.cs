using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WorkFlow.Domains
{
    /// <summary>
    /// 表单设计器控件
    /// </summary>
    public class FormDesignerItem
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Padding { get; set; }
        public string SelectValues { get; set; }
        public string Span { get; set; }
        public string Color { get; set; }
        public string Background { get; set; }
        public string Align { get; set; }
        public string Name { get; set; }
        public string FormName { get; set; }
        public string Tips { get; set; }
        public bool Required { get; set; }
        public List<FormDesignerItem> Children { get; set; }
    }
}
