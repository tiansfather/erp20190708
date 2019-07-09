using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Master.Authentication
{
    public class UserClaim : CreationAuditedEntity<int>, IMayHaveTenant
    {
        
        public UserClaim()
        {

        }
        public UserClaim(User user, Claim claim)
        {
            TenantId = user.TenantId;
            UserId = user.Id;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public virtual int? TenantId { get; set; }
        public virtual long UserId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
    }
}
