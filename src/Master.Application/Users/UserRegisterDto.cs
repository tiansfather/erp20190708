using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Users
{
    /// <summary>
    /// 用户通过第三方注册，如微信
    /// </summary>
    public class ExternalUserRegisterDto
    {
        public int TenantId { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public int? OrganizationId { get; set; }
        public int[] RoleIds { get; set; }
    }
}
