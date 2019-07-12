using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Storage
{
    public class MaterialDIYSubmitDto
    {
        public int Id { get; set; }
        public IEnumerable<DIYInfo> DIYInfos { get; set; }
    }
}
