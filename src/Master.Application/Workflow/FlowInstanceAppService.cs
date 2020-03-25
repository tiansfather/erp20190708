using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Master.Authentication;
using Master.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.WorkFlow
{
    [AbpAuthorize]
    public class FlowInstanceAppService : ModuleDataAppServiceBase<FlowInstance, int>
    {
        public IAbpStartupConfiguration Configuration { get; set; }
        public IRepository<FlowInstanceTransitionHistory,int> FlowInstanceTransitionHistoryRepository { get; set; }
        public IRepository<FlowInstanceOperationHistory,int> FlowInstanceOperationHistoryRepository { get; set; }
        public IRepository<UserRole,int> UserRoleRepository { get; set; }
        public FlowSchemeManager FlowSchemeManager { get; set; }
        public FlowFormManager FlowFormManager { get; set; }
        protected override string ModuleKey()
        {
            return nameof(FlowInstance);
        }

        protected override async Task<IQueryable<FlowInstance>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<FlowInstance> query)
        {
            var userId = AbpSession.GetUserId();
            if (searchKeys.ContainsKey("type"))
            {
                var type = searchKeys["type"];
                if (type == "all")//我的流程
                {
                    query = query.Where(o => o.CreatorUserId == userId);
                    
                }else if (type == "disposed")//已办事项（即我参与过的流程）
                {
                    var intanceIds = FlowInstanceTransitionHistoryRepository.GetAll().Where(o => o.CreatorUserId == userId)
                        .Select(o => o.FlowInstanceId).Distinct();

                    query = from flowinstance in query
                            join instanceId in intanceIds on flowinstance.Id equals instanceId
                            select flowinstance;
                }
                else if(type=="processing") //待办事项
                {
                    query = query.Where(o =>(o.InstanceStatus==InstanceStatus.Processing || o.InstanceStatus==InstanceStatus.Reject) && (o.MakerList == "ALL" || o.MakerList.Contains($",{userId},")));
                }
            }
            return query;
        }

        #region 流程处理API

        /// <summary>
        /// 创建一个实例
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> CreateInstance(FlowInstanceCreateDto flowInstanceCreateDto)
        {
            var manager = Manager as FlowInstanceManager;
            var user =await GetCurrentUserAsync();
            var flowInstance = flowInstanceCreateDto.MapTo<FlowInstance>();
            flowInstance.Code = Common.Fun.ConvertToTimeStamp(DateTime.Now).ToString();
            FlowForm form = null;
            //1.如果流程提交数据没有FlowSchemeId，表示此流程直接由表单创建，提交即代表完成
            if (flowInstanceCreateDto.FlowSchemeId == null || flowInstanceCreateDto.FlowSchemeId.Value==0)
            {
                if (flowInstanceCreateDto.FlowFormId == null || flowInstanceCreateDto.FlowFormId.Value==0)
                {
                    throw new UserFriendlyException(L("参数错误,必须提供FlowFormId参数"));
                }
                form = await FlowFormManager.GetByIdFromCacheAsync(flowInstanceCreateDto.FlowFormId.Value);
                //如果表单没有内容，默认从内置表单中获取
                if (string.IsNullOrWhiteSpace(form.FormContent))
                {
                    form.FormContent = Configuration.Modules.Core().DefaultForms.SingleOrDefault(o => o.FormKey == form.FormKey)?.FormContent;
                }
                flowInstance.FlowSchemeId = null;
                flowInstance.FormContent = form.FormContent;
                flowInstance.FormType = form.FormType;
                flowInstance.InstanceName = form.FormName;
                flowInstance.InstanceStatus = InstanceStatus.Finish;//直接为完成状态

                await manager.CreateInstance(flowInstance);
                await CurrentUnitOfWork.SaveChangesAsync();
                await manager.FinishInstance(flowInstance);//调用流程结束事件
                return flowInstance.Id;
            }

            //2.从流程定义中复制表单id及流程内容
            var flowScheme = await FlowSchemeManager.GetAll().Include(o=>o.FlowForm).Where(o=>o.Id==flowInstanceCreateDto.FlowSchemeId).SingleAsync();
            form = await FlowFormManager.GetByIdFromCacheAsync(flowScheme.FlowFormId);
            flowInstance.FlowFormId = flowScheme.FlowFormId;
            flowInstance.FormContent = form.FormContent;
            flowInstance.FormType = flowScheme.FlowForm.FormType;
            flowInstance.SchemeContent = flowScheme.SchemeContent;
            flowInstance.InstanceName = flowScheme.SchemeName;
            
            //创建运行实例
            var wfruntime = new FlowRuntime(flowInstance);

            #region 根据运行实例改变当前节点状态

            flowInstance.ActivityId = wfruntime.nextNodeId;
            flowInstance.ActivityType = wfruntime.GetNextNodeType();
            flowInstance.ActivityName = wfruntime.nextNode.name;
            flowInstance.PreviousId = wfruntime.currentNodeId;
            flowInstance.MakerList = (wfruntime.GetNextNodeType() != 4 ? GetNextMakers(wfruntime) : "");
            flowInstance.InstanceStatus = (wfruntime.GetNextNodeType() == 4 ? InstanceStatus.Finish : InstanceStatus.Processing);

            await manager.CreateInstance(flowInstance);
            //await CurrentUnitOfWork.SaveChangesAsync();
            if (flowInstance.InstanceStatus == InstanceStatus.Finish)
            {
                await manager.FinishInstance(flowInstance);//调用流程结束事件
            }
            wfruntime.flowInstanceId = flowInstance.Id;

            #endregion 根据运行实例改变当前节点状态

            #region 流程操作记录

            FlowInstanceOperationHistory processOperationHistoryEntity = new FlowInstanceOperationHistory
            {
                FlowInstanceId = flowInstance.Id,
                Content = "【创建】"
                          + user.Name
                          + "创建了一个流程进程【"
                          + flowInstance.Code + "/"
                          + flowInstance.InstanceName + "】"
            };
            await FlowInstanceOperationHistoryRepository.InsertAsync(processOperationHistoryEntity);

            #endregion 流程操作记录

            await AddTransHistory(wfruntime);

            return flowInstance.Id;
        }

        /// <summary>
        /// 重发一个实例
        /// </summary>
        /// <param name="flowInstanceRepostDto"></param>
        /// <returns></returns>
        public virtual async Task RepostInstance(FlowInstanceRepostDto flowInstanceRepostDto)
        {
            var manager = Manager as FlowInstanceManager;
            var user = await GetCurrentUserAsync();
            var flowInstance = await manager.GetByIdAsync(flowInstanceRepostDto.Id);
            flowInstance.FormData = flowInstanceRepostDto.FormData;//读取表单数据

            //创建运行实例
            var wfruntime = new FlowRuntime(flowInstance);

            #region 根据运行实例改变当前节点状态

            flowInstance.ActivityId = wfruntime.nextNodeId;
            flowInstance.ActivityType = wfruntime.GetNextNodeType();
            flowInstance.ActivityName = wfruntime.nextNode.name;
            flowInstance.PreviousId = wfruntime.currentNodeId;
            flowInstance.MakerList = (wfruntime.GetNextNodeType() != 4 ? GetNextMakers(wfruntime) : "");
            flowInstance.InstanceStatus = (wfruntime.GetNextNodeType() == 4 ? InstanceStatus.Finish : InstanceStatus.Processing);

            await manager.UpdateAsync(flowInstance);
            await CurrentUnitOfWork.SaveChangesAsync();
            if (flowInstance.InstanceStatus == InstanceStatus.Finish)
            {
                await manager.FinishInstance(flowInstance);//调用流程结束事件
            }
            wfruntime.flowInstanceId = flowInstance.Id;

            #endregion 根据运行实例改变当前节点状态

            #region 流程操作记录

            FlowInstanceOperationHistory processOperationHistoryEntity = new FlowInstanceOperationHistory
            {
                FlowInstanceId = flowInstance.Id,
                Content = "【重发】"
                          + user.Name
                          + "重发了一个流程进程【"
                          + flowInstance.Code + "/"
                          + flowInstance.InstanceName + "】"
            };
            await FlowInstanceOperationHistoryRepository.InsertAsync(processOperationHistoryEntity);

            #endregion 流程操作记录

            await AddTransHistory(wfruntime);
        }

        /// <summary>
        /// 节点审核
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public virtual async Task NodeVerification(int instanceId, Tag tag)
        {
            var manager = Manager as FlowInstanceManager;
            FlowInstance flowInstance = await manager.GetByIdAsync(instanceId);
            FlowInstanceOperationHistory flowInstanceOperationHistory = new FlowInstanceOperationHistory
            {
                FlowInstanceId = instanceId
            };//操作记录
            FlowRuntime wfruntime = new FlowRuntime(flowInstance);

            #region 会签

            if (flowInstance.ActivityType == 0)//当前节点是会签节点
            {
                //TODO: 标记会签节点的状态，这个地方感觉怪怪的
                wfruntime.MakeTagNode(wfruntime.currentNodeId, tag);

                string canCheckId = ""; //寻找当前登录用户可审核的节点Id
                foreach (string nodeId in wfruntime.FromNodeLines[wfruntime.currentNodeId].Select(u => u.to))
                {
                    var makerList = GetNodeMakers(wfruntime.Nodes[nodeId]);
                    if (string.IsNullOrEmpty(makerList)) continue;

                    if (makerList.Contains("|"+tag.UserId+"|"))
                    {
                        canCheckId = nodeId;
                    }
                }

                if (canCheckId == "")
                {
                    throw (new Exception("审核异常,找不到审核节点"));
                }

                flowInstanceOperationHistory.Content = "【" + wfruntime.Nodes[canCheckId].name
                                                           + "】【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                                                           + "】" + (tag.Taged == TagState.Ok ? "同意" : "不同意") + ",备注："
                                                           + tag.Description;

                wfruntime.MakeTagNode(canCheckId, tag); //标记审核节点状态
                string res = wfruntime.NodeConfluence(canCheckId, tag);
                if (res == TagState.No.ToString("D"))
                {
                    flowInstance.InstanceStatus = InstanceStatus.DisAgree;
                }
                else if (!string.IsNullOrEmpty(res))
                {
                    flowInstance.PreviousId = flowInstance.ActivityId;
                    flowInstance.ActivityId = wfruntime.nextNodeId;
                    flowInstance.ActivityType = wfruntime.nextNodeType;
                    flowInstance.ActivityName = wfruntime.nextNode.name;
                    flowInstance.InstanceStatus = (wfruntime.nextNodeType == 4 ? InstanceStatus.Finish : InstanceStatus.Processing);
                    flowInstance.MakerList =
                        (wfruntime.nextNodeType == 4 ? "" : GetNextMakers(wfruntime));

                    await AddTransHistory(wfruntime);
                }

            }
            #endregion 会签

            #region 一般审核

            else
            {
                wfruntime.MakeTagNode(wfruntime.currentNodeId, tag);
                if (tag.Taged == TagState.Ok)
                {
                    flowInstance.PreviousId = flowInstance.ActivityId;
                    flowInstance.ActivityId = wfruntime.nextNodeId;
                    flowInstance.ActivityType = wfruntime.nextNodeType;
                    flowInstance.ActivityName = wfruntime.nextNode.name;
                    flowInstance.MakerList = wfruntime.nextNodeType == 4 ? "" : GetNextMakers(wfruntime);
                    flowInstance.InstanceStatus = (wfruntime.nextNodeType == 4 ? InstanceStatus.Finish : InstanceStatus.Processing);
                    await AddTransHistory(wfruntime);
                }
                else
                {
                    flowInstance.InstanceStatus = InstanceStatus.DisAgree;  //表示该节点不同意
                }
                flowInstanceOperationHistory.Content = "【" + wfruntime.currentNode.name
                                                           + "】【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                                                           + "】" + (tag.Taged == TagState.Ok ? "同意" : "不同意") + ",备注："
                                                           + tag.Description;
            }

            #endregion 一般审核

            flowInstance.SchemeContent = Newtonsoft.Json.JsonConvert.SerializeObject(wfruntime.ToSchemeObj());

            await manager.UpdateAsync(flowInstance);
            if (flowInstance.InstanceStatus == InstanceStatus.Finish)
            {
                await manager.FinishInstance(flowInstance);//调用流程结束事件
            }
            await FlowInstanceOperationHistoryRepository.InsertAsync(flowInstanceOperationHistory);
        }

        /// <summary>
        /// 驳回
        /// </summary>
        /// <returns></returns>
        public virtual async Task NodeReject(FlowVerifySubmitDto reqest)
        {
            var user = await GetCurrentUserAsync();

            FlowInstance flowInstance =await Manager.GetByIdAsync(reqest.FlowInstanceId);

            FlowRuntime wfruntime = new FlowRuntime(flowInstance);

            string resnode = "";
            resnode = string.IsNullOrEmpty(reqest.NodeRejectStep) ? wfruntime.RejectNode() : reqest.NodeRejectStep;

            var tag = new Tag
            {
                Description = reqest.VerificationOpinion,
                Taged = TagState.Reject,
                UserId = user.Id,
                UserName = user.Name
            };

            wfruntime.MakeTagNode(wfruntime.currentNodeId, tag);
            flowInstance.InstanceStatus = InstanceStatus.Reject;//4表示驳回（需要申请者重新提交表单）
            if (resnode != "")
            {
                var currentNode = wfruntime.Nodes[resnode];
                flowInstance.PreviousId = flowInstance.ActivityId;
                flowInstance.ActivityId = resnode;
                flowInstance.ActivityType = wfruntime.GetNodeType(resnode);
                flowInstance.ActivityName = currentNode.name;
                //如果是开始节点，则取流程实例的创建者
                flowInstance.MakerList = flowInstance.ActivityType==3?$",{flowInstance.CreatorUserId},": GetNodeMakers(currentNode);//当前节点可执行的人信息

                await AddTransHistory(wfruntime);
            }
            flowInstance.SchemeContent = Newtonsoft.Json.JsonConvert.SerializeObject(wfruntime.ToSchemeObj());

            await Manager.UpdateAsync(flowInstance);

            await FlowInstanceOperationHistoryRepository.InsertAsync(new FlowInstanceOperationHistory
            {
                FlowInstanceId = reqest.FlowInstanceId,
                Content = "【"
                          + wfruntime.currentNode.name
                          + "】【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "】驳回,备注："
                          + reqest.VerificationOpinion
            });

        }

        #endregion 流程处理API

        #region Private

        /// <summary>
        /// 寻找下一步的执行人
        /// 一般用于本节点审核完成后，修改流程实例的当前执行人，可以做到通知等功能
        /// </summary>
        /// <returns></returns>
        private string GetNextMakers(FlowRuntime wfruntime)
        {
            string makerList = "";
            if (wfruntime.nextNodeId == "-1")
            {
                throw (new Exception("无法寻找到下一个节点"));
            }
            if (wfruntime.nextNodeType == 0)//如果是会签节点
            {
                List<string> _nodelist = wfruntime.FromNodeLines[wfruntime.nextNodeId].Select(u => u.to).ToList();
                string makers = "";
                foreach (string item in _nodelist)
                {
                    makers = GetNodeMakers(wfruntime.Nodes[item]);
                    if (makers == "")
                    {
                        throw (new Exception("无法寻找到会签节点的审核者,请查看流程设计是否有问题!"));
                    }
                    if (makers == "ALL")
                    {
                        throw (new Exception("会签节点的审核者不能为所有人,请查看流程设计是否有问题!"));
                    }
                    if (makerList != "")
                    {
                        makerList += ",";
                    }
                    makerList += makers;
                }
            }
            else
            {
                makerList = GetNodeMakers(wfruntime.nextNode);
                if (string.IsNullOrEmpty(makerList))
                {
                    throw (new Exception("无法寻找到节点的审核者,请查看流程设计是否有问题!"));
                }
            }
            return makerList;
        }

        /// <summary>
        /// 寻找该节点执行人
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetNodeMakers(FlowNode node)
        {
            string makerList = "";

            if (node.setInfo != null)
            {
                if (node.setInfo.NodeDesignate == Setinfo.ALL_USER)//所有成员
                {
                    makerList = "ALL";
                }
                else if (node.setInfo.NodeDesignate == Setinfo.SPECIAL_USER)//指定成员
                {
                    makerList =","+string.Join(',', node.setInfo.NodeDesignateData.users)+",";
                }
                else if (node.setInfo.NodeDesignate == Setinfo.SPECIAL_ROLE)  //指定角色
                {
                    var users=UserRoleRepository.GetAll().Where(o => node.setInfo.NodeDesignateData.roles.Contains(o.RoleId)).Select(o => o.UserId).ToList();
                    makerList = "," + string.Join(',', users) + ",";
                }
            }
            return makerList;
        }
        /// <summary>
        /// 添加扭转记录
        /// </summary>
        private async Task AddTransHistory(FlowRuntime wfruntime)
        {
            await FlowInstanceTransitionHistoryRepository.InsertAsync(new FlowInstanceTransitionHistory
            {
                FlowInstanceId = wfruntime.flowInstanceId,
                FromNodeId = wfruntime.currentNodeId,
                FromNodeName = wfruntime.currentNode.name,
                FromNodeType = wfruntime.currentNodeType,
                ToNodeId = wfruntime.nextNodeId,
                ToNodeName = wfruntime.nextNode.name,
                ToNodeType = wfruntime.nextNodeType,
                InstanceStatus = wfruntime.nextNodeType == 4 ? InstanceStatus.Finish : InstanceStatus.Processing,
                TransitionSate = 0
            });
        }


        #endregion

        /// <summary>
        /// 获取流程实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<FlowInstanceDto> Get(int id)
        {
            return (await Manager.GetByIdFromCacheAsync(id)).MapTo<FlowInstanceDto>();
        }
        /// <summary>
        /// 审核流程
        /// <para>李玉宝于2017-01-20 15:44:45</para>
        /// </summary>
        public virtual async Task Verification(FlowVerifySubmitDto request)
        {
            var user =await GetCurrentUserAsync();
            var tag = new Tag
            {
                UserName = user.Name,
                UserId = user.Id,
                Description = request.VerificationOpinion,
                Taged = request.VerificationFinally
            };
            //驳回
            if (request.VerificationFinally == TagState.Reject)
            {
                await NodeReject(request);
            }
            else
            {
                await NodeVerification(request.FlowInstanceId, tag);
            }
        }

        
    }
}
