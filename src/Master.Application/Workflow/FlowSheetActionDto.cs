using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Workflow
{
    public class FlowSheetActionDto
    {
        public int SheetId { get; set; }
        public string ActionName { get; set; }
        public string FormData { get; set; }
        public DateTime? LastModifyTime { get; set; }
    }
}
