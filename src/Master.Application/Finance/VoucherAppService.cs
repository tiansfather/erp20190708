using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.UI;
using Master.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Finance
{
    [AbpAuthorize]
    public class VoucherAppService : ModuleDataAppServiceBase<Voucher, int>
    {
        protected override async Task<IQueryable<Voucher>> GetQueryable(RequestPageDto request)
        {
            var user = await GetCurrentUserAsync();
            return (await base.GetQueryable(request))
                .WhereIf(user.UnitId.HasValue, o => o.UnitId == user.UnitId)//代理商登录只看到自己;
                ;
        }
        protected override string ModuleKey()
        {
            return nameof(Voucher);
        }

        public virtual async Task Submit(VoucherDto voucherDto)
        {
            var voucher = voucherDto.MapTo<Voucher>();
            await Manager.InsertAsync(voucher);
        }

        public override async Task<object> GetById(int primary)
        {
            var entity = await Manager.GetAll().Include(o => o.Unit).Where(o => o.Id == primary).SingleOrDefaultAsync();
            if (entity == null)
            {
                throw new UserFriendlyException("数据已被删除,请联系管理员");
            }
            return new
            {
                entity.Unit.UnitName,
                entity.Fee,
                entity.RelSheetSN,
                entity.Remarks,
                entity.FilePath,
                CreationTime=entity.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                entity.VoucherStatus,
                VoucherStatusName=entity.VoucherStatus.ToString()
            };
        }

        public virtual async Task SetVoucherStatus(int id,VoucherStatus voucherStatus)
        {
            var entity = await Manager.GetByIdAsync(id);
            entity.VoucherStatus = voucherStatus;
        }

        public virtual async Task SetRelSheet(int id,string sheetSN)
        {
            var entity = await Manager.GetByIdAsync(id);
            entity.RelSheetSN = sheetSN;
        }
    }
}
