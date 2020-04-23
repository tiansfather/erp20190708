using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Entity;
using Master.Module;
using Master.Module.Attributes;
using Master.MultiTenancy;
using Master.Organizations;
using Master.Units;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Authentication
{
    [InterModule("员工信息",GenerateOperateColumn =true)]
    public class User:FullAuditedEntity<long>, IMayHaveTenant,  IPassivable, IExtendableObject,IAutoEntity,IMayHaveOrganization,IHaveProperty,IHaveStatus
    {
        public const string Status_NotVerified = "NotVerified";
        public const string AdminUserName= "admin";
        public const string AdminUserPassword = "12345678";
        public virtual int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        [InterColumn(ColumnName = "账号",Sort = 0,IsShownInAdd =false,IsShownInEdit =false,IsShownInMultiEdit =false,IsShownInAdvanceSearch =false)]
        public virtual string UserName { get; set; }
        [InterColumn(ColumnName = "姓名", VerifyRules = "required", Sort =0)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [MaxLength(2)]
        [InterColumn(ColumnName = "性别", ColumnType = ColumnTypes.Select, DictionaryName = StaticDictionaryNames.Sex, ControlFormat = "radio", DefaultValue = "\"男\"",Sort=1)]
        public virtual string Sex { get; set; }
        public virtual string Password { get; set; }
        [InterColumn(ColumnName = "手机号码", Sort = 2)]
        public virtual string PhoneNumber { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        [InterColumn(ColumnName = "工号", Sort = 3)]
        public virtual string WorkNumber { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        [DataType(DataType.Date)]
        [InterColumn(ColumnName = "出生日期", ColumnType = ColumnTypes.DateTime, ControlFormat = "date", DisplayFormat = "yyyy-MM-dd", Sort = 4)]
        public virtual DateTime? BirthDay { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        [InterColumn(ColumnName = "学历", ColumnType = ColumnTypes.Select, DictionaryName = StaticDictionaryNames.Degree, ControlFormat = "select", Sort = 5)]
        public virtual string Degree { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        [InterColumn(ColumnName = "民族", Sort = 6)]
        public virtual string Nation { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        [DataType(DataType.Date)]
        [InterColumn(ColumnName = "入职日期", ColumnType = ColumnTypes.DateTime, ControlFormat = "date", DisplayFormat = "yyyy-MM-dd", DefaultValue = "DateTime.Now.ToString(\"yyyy-MM-dd\")", Sort = 7)]
        public virtual DateTime? JobDateStart { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        [DataType(DataType.Date)]
        [InterColumn(ColumnName = "离职日期", ColumnType = ColumnTypes.DateTime, ControlFormat = "date", Sort = 8)]
        public virtual DateTime? JobDateEnd { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        [InterColumn(ColumnName = "婚姻状况", ColumnType = ColumnTypes.Select, DictionaryName = StaticDictionaryNames.Marriage, ControlFormat = "radio", Sort = 9)]
        public virtual string Marriage { get; set; }
        public virtual bool IsActive { get; set; } = true;
        [ForeignKey("UserId")]
        public virtual ICollection<UserPermissionSetting> Permissions { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<UserLogin> Logins { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> Roles { get; set; }
        public virtual DateTime? LastLoginTime { get; set; }
        public virtual string ExtensionData { get; set; }
        public virtual User CreatorUser { get; set; }
        public virtual User LastModifierUser { get; set; }
        public virtual User DeleterUser { get; set; }
        [InterColumn(ColumnName ="部门",ColumnType =ColumnTypes.Text,Renderer ="lay-departchoose",DisplayPath ="Organization.DisplayName",Templet = "{{d.organizationId_display?d.organizationId_display:''}}", Sort = 10)]
        public int? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        [Column(TypeName ="json")]
        public JsonObject<IDictionary<string, object>> Property { get; set; }

        public int? UnitId { get; set; }
        public virtual Unit Unit { get; set; }

        #region 非绑定数据
        /// <summary>
        /// 电子签名地址
        /// </summary>
        [NotMapped]
        public string Signature
        {
            get
            {
                return this.GetPropertyValue<string>("Signature");
            }
            set
            {
                this.SetPropertyValue("Signature", value);
            }
        }
        /// <summary>
        /// 是否独立用户,独立用户只能查看自己录入的信息
        /// </summary>
        [NotMapped]
        public bool IsSeparate
        {
            get
            {
                return this.GetPropertyValue<bool>("Separate");
            }
            set
            {
                this.SetPropertyValue("Separate", value);
            }
        }
        /// <summary>
        /// 用户是否未审核
        /// </summary>
        [NotMapped]
        public bool ToBeVerified
        {
            get
            {
                return this.HasStatus(Status_NotVerified);
            }
            set
            {
                if (value)
                {
                    this.SetStatus(Status_NotVerified);
                }
                else
                {
                    this.RemoveStatus(Status_NotVerified);
                }

            }
        }
        #endregion

        public string Status { get;set; }

        /// <summary>
        /// 生成账套管理员用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static User CreateTenantAdminUser(int tenantId)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Password=Abp.Runtime.Security.SimpleStringCipher.Instance.Encrypt(AdminUserPassword)
            };

            return user;
        }

        /// <summary>
        /// 生成主体管理员用户
        /// </summary>
        /// <returns></returns>
        public static User CreateHostAdminUser()
        {
            var user = new User
            {
                TenantId = null,
                UserName = AdminUserName,
                Name = AdminUserName,
                Password = Abp.Runtime.Security.SimpleStringCipher.Instance.Encrypt(AdminUserPassword)
            };

            return user;
        }

        public UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(TenantId, Id);
        }

        [NotMapped]
        public bool IsCenterUser
        {
            get
            {
                return !UnitId.HasValue;
            }
        }
    }
    public class UserEntityMapConfiguration : EntityMappingConfiguration<User>
    {
        public override void Map(EntityTypeBuilder<User> b)
        {
            b.HasOne(p => p.DeleterUser)
                .WithMany()
                .HasForeignKey(p => p.DeleterUserId);

            b.HasOne(p => p.CreatorUser)
                .WithMany()
                .HasForeignKey(p => p.CreatorUserId);

            b.HasOne(p => p.LastModifierUser)
                .WithMany()
                .HasForeignKey(p => p.LastModifierUserId);
            b.HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId);

            b.HasOne(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId);
        }
    }
}
