using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.UI;
using Master.Configuration;
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
    public class FlowFormAppService : ModuleDataAppServiceBase<FlowForm, int>
    {
        public IAbpStartupConfiguration Configuration { get; set; }
        protected override string ModuleKey()
        {
            return nameof(FlowForm);
        }
        /// <summary>
        /// 获取所有表单
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<FormSimpleDto>> GetAll()
        {
            var manager = Manager as FlowFormManager;
            return (await manager.GetAllList()).Where(o=>o.IsActive).OrderBy(o=>o.Sort).MapTo<List<FormSimpleDto>>();
        }

        #region Designer
        /// <summary>
        /// 获取设计表单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<FormDesignerDto> GetFormDesigner(int id)
        {
            

            var form = await Manager.GetByIdFromCacheAsync(id);
            var dto = form.MapTo<FormDesignerDto>();
            try
            {
                dto.Datas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FormDesignerItem>>(form.FormContent);
            }
            catch
            {

            }

            return dto;
        }

        /// <summary>
        /// 提交表单设计
        /// </summary>
        /// <param name="formDesignerDto"></param>
        /// <returns></returns>
        public virtual async Task SubmitFormDesigner(FormDesignerDto formDesignerDto)
        {
            var manager = Manager;
            var form = await manager.GetByIdAsync(formDesignerDto.Id);
            formDesignerDto.MapTo(form);
            form.FormContent = Newtonsoft.Json.JsonConvert.SerializeObject(formDesignerDto.Datas);
        }
        #endregion

        #region Html
        /// <summary>
        /// 获取Html表单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<FormHtmlDto> GetFormHtml(int id)
        {
            var form = await Manager.GetByIdFromCacheAsync(id);
            return form.MapTo<FormHtmlDto>();
        }

        /// <summary>
        /// 提交表单Html
        /// </summary>
        /// <param name="formHtmlDto"></param>
        /// <returns></returns>
        public virtual async Task SubmitFormHtml(FormHtmlDto formHtmlDto)
        {
            var manager = Manager;
            var form = await manager.GetByIdAsync(formHtmlDto.Id);
            formHtmlDto.MapTo(form);
        }
        #endregion

        #region Handler
        /// <summary>
        /// 获取表单处理程序内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<FormHandlerDto> GetFormHandler(int id)
        {
            var form = await Manager.GetByIdFromCacheAsync(id);
            return form.MapTo<FormHandlerDto>();
        }

        /// <summary>
        /// 提交表单Html
        /// </summary>
        /// <param name="formHtmlDto"></param>
        /// <returns></returns>
        public virtual async Task SubmitFormHandler(FormHandlerDto formHtmlDto)
        {
            var manager = Manager;
            var form = await manager.GetByIdAsync(formHtmlDto.Id);
            formHtmlDto.MapTo(form);
        }
        #endregion

        #region 删除
        public override async Task DeleteEntity(IEnumerable<int> ids)
        {
            if(await Manager.GetAll().CountAsync(o=>ids.Contains(o.Id) && o.IsSystemForm) > 0)
            {
                throw new UserFriendlyException("系统表单无法删除");
            }
            if(await Resolve<FlowInstanceManager>().GetAll().CountAsync(o => ids.Contains(o.FlowFormId)) > 0)
            {
                throw new UserFriendlyException("表单已被使用,无法删除");
            }
            await base.DeleteEntity(ids);
        }
        #endregion

        /// <summary>
        /// 对某个账套进行初始化表单
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public virtual async Task InitDefaultForm(int? tenantId)
        {
            var defaultForms = Configuration.Modules.WorkFlow().DefaultForms;
            var setTenantId = tenantId ?? AbpSession.TenantId;
            using (CurrentUnitOfWork.SetTenantId(setTenantId))
            {
                //var oriForms = await Manager.GetAll().ToListAsync();
                var oriForms = await Manager.GetAllList();
                foreach(var newForm in defaultForms)
                {
                    var oriForm = oriForms.FirstOrDefault(o => o.FormKey == newForm.FormKey);
                    if (oriForm==null)
                    {
                        var form = new FlowForm()
                        {
                            FormKey = newForm.FormKey,
                            IsSystemForm = true,
                            FormName = newForm.FormName,
                            FormType = newForm.FormType
                        };
                        //var formToBeAdd = newForm.MapTo<FlowForm>();
                        //formToBeAdd.IsSystemForm = true;
                        //formToBeAdd.FormContent = "";//清空表单内容，约定当表单内容为空时读取内置表单内容
                        await Manager.InsertAsync(form);
                    }
                    //else
                    //{
                    //    //如果表单内容为空,则将默认表单内容填入
                    //    oriForm.FormContent = newForm.FormContent;
                    //}
                }
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }
    }
}
