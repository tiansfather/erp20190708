using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Projects
{
    [AbpAuthorize]
    public class ProjectAppService : ModuleDataAppServiceBase<Project, int>
    {
        public virtual async Task<List<ProjectDto>> GetAll(string key,int take=50)
        {
            var query = Manager.GetAll();
            if (!key.IsNullOrEmpty())
            {
                query = query.Where(o => o.ProjectSN.Contains(key));
            }

            return (await query.OrderByDescending(o => o.Id).Take(take).ToListAsync()).MapTo<List<ProjectDto>>();
        }

        protected override string ModuleKey()
        {
            return nameof(Project);
        }
    }
}
