using Abp.Authorization;
using Abp.AutoMapper;
using Master.WorkFlow.Domains;
using Master.WorkFlow.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.WorkFlow.Services
{
    [AbpAuthorize]
    public class FlowSchemeAppService : ModuleDataAppServiceBase<FlowScheme, int>
    {
        public FlowFormManager FlowFormManager { get; set; }
        protected override string ModuleKey()
        {
            return nameof(FlowScheme);
        }

        /// <summary>
        /// 获取可用流程
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<FlowSchemeDto>> GetAvailableFlowSchemes()
        {
            var manager = Manager as FlowSchemeManager;
            var flowSchemes = await manager.GetAllList();
            //todo:需要进行当前用户权限判断
            //获取流程定义
            var schemes= flowSchemes.Where(o=>o.IsActive).OrderBy(o=>o.Sort).MapTo<List<FlowSchemeDto>>();
            //获取没有流程定义的表单
            var formsWithNoSchemes = await FlowFormManager.GetAll().Where(o => !flowSchemes.Exists(f => f.FlowFormId == o.Id)).ToListAsync();
            schemes.AddRange(formsWithNoSchemes.Select(o=>new FlowSchemeDto()
            {
                FlowFormId=o.Id,
                Id=0,
                SchemeName=o.FormName
            }));
            return schemes;
        }

        /// <summary>
        /// 获取流程定义
        /// </summary>
        /// <param name="schemeId"></param>
        /// <returns></returns>
        public virtual async Task<FlowSchemeDto> Get(int schemeId)
        {
            return (await Manager.GetByIdFromCacheAsync(schemeId)).MapTo<FlowSchemeDto>();
        }
        /// <summary>
        /// 更新流程定义
        /// </summary>
        /// <param name="flowSchemeDto"></param>
        /// <returns></returns>
        public virtual async Task Update(FlowSchemeDto flowSchemeDto)
        {
            var manager = Manager;
            FlowScheme flowScheme = null;
            if (flowSchemeDto.Id == 0)
            {
                flowScheme = flowSchemeDto.MapTo<FlowScheme>();
                await manager.InsertAsync(flowScheme);
            }
            else
            {
                flowScheme = await manager.GetByIdAsync(flowSchemeDto.Id);
                flowSchemeDto.MapTo(flowScheme);
                await manager.UpdateAsync(flowScheme);
            }
        }
    }
}
