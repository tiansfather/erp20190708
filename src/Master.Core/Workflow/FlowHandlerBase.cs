using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.UI;
using Master.Authentication;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.WorkFlow
{
    /// <summary>
    /// 表单处理基类
    /// </summary>
    public abstract class FlowHandlerBase : IFlowHandler
    {
        public FlowSheetManager FlowSheetManager { get; set; }
        public FlowInstanceManager FlowInstanceManager { get; set; }
        public UserManager UserManager { get; set; }
        public IAbpSession AbpSession { get; set; }
        public User CurrentUser
        {
            get
            {
                return UserManager.GetByIdAsync(AbpSession.UserId.Value).Result;
            }
        }
        public virtual async Task<FlowSheet> CreateSheet(FlowInstance instance, FlowForm flowForm)
        {
            var formData = instance.FormData;
            var formKey = flowForm.FormKey;
            //生成单据
            var flowSheet = new FlowSheet()
            {
                FlowInstanceId = instance.Id,
                SheetName = instance.InstanceName,
                FormKey = formKey,
                SheetNature=SheetNature.正单
            };
            var sheetId = await FlowSheetManager.InsertAndGetIdAsync(flowSheet);
            return flowSheet;
        }

        public virtual async Task Handle(FlowSheet flowSheet)
        {
            flowSheet.SheetNature = SheetNature.正单;
        }

        public async Task CreateRevertSheet(FlowSheet flowSheet,string revertReason)
        {
            //产生新的流程实例
            var instance = flowSheet.FlowInstance;
            var newInstance = new FlowInstance()
            {
                InstanceName = instance.InstanceName,
                FormContent = instance.FormContent,
                FormData = instance.FormData,
                FlowFormId = instance.FlowFormId,
                FormType = instance.FormType,
                InstanceStatus = instance.InstanceStatus,
                IsActive = true,
                Code = Common.Fun.ConvertToTimeStamp(DateTime.Now).ToString()
            };
            var newInstanceId = await FlowInstanceManager.InsertAndGetIdAsync(newInstance);
            //生成新的单据
            var newFlowSheet = new FlowSheet()
            {
                UnitId=flowSheet.UnitId,
                FlowInstanceId = newInstanceId,
                SheetSN = FlowSheetManager.GenerateSheetSN(flowSheet.FormKey),
                SheetName = newInstance.InstanceName,
                FormKey = flowSheet.FormKey,
                SheetNature = SheetNature.冲红,
                RelSheetId = flowSheet.Id,
                RevertReason = revertReason,
                Property=flowSheet.Property
            };
            newFlowSheet.SheetDate = flowSheet.SheetDate;
            newFlowSheet.Remarks = flowSheet.Remarks;
            var newSheetId = await FlowSheetManager.InsertAndGetIdAsync(newFlowSheet);
            //设置旧单据状态
            flowSheet.SheetNature = SheetNature.被冲红;
            flowSheet.RelSheetId = newSheetId;
            flowSheet.RevertReason = revertReason;

            await HandleRevert(newFlowSheet);
        }

        public abstract Task HandleRevert(FlowSheet flowSheet);

        protected T Resolve<T>()
        {
            using (var wrapper=IocManager.Instance.ResolveAsDisposable<T>())
            {
                return wrapper.Object;
            }
        }

        public virtual async Task Action(FlowSheet flowSheet, string action)
        {
            
        }

        public virtual async Task<IEnumerable<ModuleButton>> GetFlowBtns(FlowSheet flowSheet)
        {
            var result = new List<ModuleButton>();
            if (flowSheet.SheetNature == SheetNature.正单)
            {
                //加入冲红按钮
                result.Add(new ModuleButton()
                {
                    ButtonKey="revert",
                    ButtonName="冲红",
                    ConfirmMsg="确认冲红此单据?",
                    ButtonClass="layui-btn-danger"
                });
            }
            return result;
        }
    }
}
