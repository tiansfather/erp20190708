using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Configuration
{
    public class WeiXinMultiAppSetting
    {
        public List<WeiXinAppSetting> WeiXinAppSettings { get; set; }
    }

    public class WeiXinAppSetting
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
