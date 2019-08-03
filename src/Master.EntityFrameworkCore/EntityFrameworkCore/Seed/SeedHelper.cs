using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using Master.Authentication;
using Master.EntityFrameworkCore.Seed.Tenants;
using Master.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Master.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<MasterDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(MasterDbContext context)
        {
            //context.Database.Migrate();
            context.SuppressAutoSetTenantId = true;
            //构建语种
            new DefaultLanguagesCreator(context).Create();
            //构建管理账号
            CreateHostRoleUser(context);
            //构建默认账套
            CreateDefaultTenant(context);
            //构建账套默认角色用户
            CreateTenantRoleUser(context, 1);
            //构建默认模块数据
            new TenantDefaultModuleBuilder(context, 1).Create();

        }
        private static void CreateHostRoleUser(MasterDbContext context)
        {
            //管理员角色
            var adminRole = context.Role.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Admin);
            if (adminRole == null)
            {
                adminRole = context.Role.Add(new Role(null, StaticRoleNames.Host.Admin, StaticRoleNames.Host.Admin) { IsStatic = true }).Entity;
                context.SaveChanges();
            }
            // 使管理员有所有权限

            var grantedPermissions = context.Permission.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            using (var provider = IocManager.Instance.ResolveAsDisposable<IPermissionManager>())
            {
                var permissions = provider.Object.GetAllPermissions(MultiTenancySides.Host);
                permissions = permissions.Where(p => !grantedPermissions.Contains(p.Name)).ToList();


                if (permissions.Any())
                {
                    context.Permission.AddRange(
                        permissions.Select(permission => new RolePermissionSetting
                        {
                            TenantId = null,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = adminRole.Id
                        })
                    );
                    context.SaveChanges();
                }
            }
            //管理用户
            var adminUser = context.User.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == null && u.UserName == User.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateHostAdminUser();
                context.User.Add(adminUser);
                context.SaveChanges();
                // Assign Admin role to admin user
                context.UserRole.Add(new UserRole(null, adminUser.Id, adminRole.Id));
                context.SaveChanges();
            }
        }
        private static void CreateTenantRoleUser(MasterDbContext context, int tenantId)
        {
            //管理员角色
            var adminRole = context.Role.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = context.Role.Add(new Role(tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                context.SaveChanges();
            }
            // 使管理员有所有权限

            var grantedPermissions = context.Permission.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            using (var provider = IocManager.Instance.ResolveAsDisposable<IPermissionManager>())
            {
                var permissions = provider.Object.GetAllPermissions(MultiTenancySides.Tenant);
                permissions=permissions.Where(p=>!grantedPermissions.Contains(p.Name)).ToList();


                if (permissions.Any())
                {
                    context.Permission.AddRange(
                        permissions.Select(permission => new RolePermissionSetting
                        {
                            TenantId = tenantId,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = adminRole.Id
                        })
                    );
                    context.SaveChanges();
                }
            }
            //管理用户
            var adminUser = context.User.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == tenantId && u.UserName == User.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(tenantId);
                context.User.Add(adminUser);
                context.SaveChanges();
                // Assign Admin role to admin user
                context.UserRole.Add(new UserRole(tenantId, adminUser.Id, adminRole.Id));
                context.SaveChanges();
            }

            //添加代理商角色
            var unitRole = context.Role.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == tenantId && r.Name == StaticRoleNames.Tenants.Unit);
            if (unitRole == null)
            {
                unitRole = context.Role.Add(new Role(tenantId, StaticRoleNames.Tenants.Unit, StaticRoleNames.Tenants.Unit) { IsStatic = true }).Entity;
                context.SaveChanges();
            }
            // 代理商角色的权限

            grantedPermissions = context.Permission.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == tenantId && p.RoleId == unitRole.Id)
                .Select(p => p.Name)
                .ToList();

            using (var provider = IocManager.Instance.ResolveAsDisposable<IPermissionManager>())
            {
                var permissionNames = new List<string>() {
                    "Menu.Sell.Tenancy.SDD",
                    "Menu.Sell.Tenancy.SDDSheet",
                    "Menu.Sell.Tenancy.SDR",
                    "Menu.Sell.Tenancy.SDRSheet",
                    "Menu.Finance.Tenancy.VoucherAdd",
                    "Menu.Finance.Tenancy.Voucher",
                    "Menu.Summary.Tenancy.SellSummary",
                    "Menu.Summary.Tenancy.SellDetail",
                    "Menu.Summary.Tenancy.UnitFeeHistory",
                };
                permissionNames = permissionNames.Where(p => !grantedPermissions.Contains(p)).ToList();


                if (permissionNames.Any())
                {
                    context.Permission.AddRange(
                        permissionNames.Select(permission => new RolePermissionSetting
                        {
                            TenantId = tenantId,
                            Name = permission,
                            IsGranted = true,
                            RoleId = unitRole.Id
                        })
                    );
                    context.SaveChanges();
                }
            }
        }

        private static void CreateDefaultTenant(MasterDbContext context)
        {
            var defaultTenant = context.Set<Tenant>().IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                defaultTenant = new Tenant(Tenant.DefaultTenantName, Tenant.DefaultTenantName);
                defaultTenant.IsActive = true;
                //defaultTenant.ConnectionString = SimpleStringCipher.Instance.Encrypt("Server=localhost; Database=MasterDb_Tenant_"+AbpTenantBase.DefaultTenantName+"; User Id=skynetsoft;password=skynetsoft");

                //var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                //if (defaultEdition != null)
                //{
                //    defaultTenant.EditionId = defaultEdition.Id;
                //}
                context.Set<Tenant>().Add(defaultTenant);
                context.SaveChanges();
            }
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
