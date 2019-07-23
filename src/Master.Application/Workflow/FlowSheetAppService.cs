﻿using Abp.Authorization;
using Master.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Workflow
{
    [AbpAuthorize]
    public class FlowSheetAppService:MasterAppServiceBase<FlowSheet,int>
    {
        /// <summary>
        /// 获取单据简要信息
        /// </summary>
        /// <param name="primary"></param>
        /// <returns></returns>
        public override async Task<object> GetById(int primary)
        {
            var flowSheet = await Manager.GetAll().Include(o => o.CreatorUser)
                .Include(o=>o.RelSheet)
                .Where(o => o.Id == primary)
                .FirstOrDefaultAsync();
            var btns = await (Manager as FlowSheetManager).GetFlowBtns(primary);
            return new
            {
                flowSheet.SheetSN,
                flowSheet.SheetNature,
                SheetNatureName = flowSheet.SheetNature.ToString(),
                CreatorUserName=flowSheet.CreatorUser.Name,
                CreationTime=flowSheet.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                flowSheet.RevertReason,
                RelSheetSN=flowSheet.RelSheet?.SheetSN,
                Btns= btns.Select(o => new
                {
                    o.ButtonKey,
                    o.ButtonName,
                    o.ButtonClass,
                    o.ConfirmMsg
                })
            };
        }

        /// <summary>
        /// 冲红单据
        /// </summary>
        /// <param name="sheetId"></param>
        /// <returns></returns>
        public virtual async Task Revert(int sheetId,string revertReason)
        {
            await (Manager as FlowSheetManager).Revert(sheetId, revertReason);
        }

        /// <summary>
        /// 单据操作
        /// </summary>
        /// <param name="sheetId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual async Task Action(int sheetId,string action)
        {
            await (Manager as FlowSheetManager).Action(sheetId, action);
        }

        public virtual async Task<object> GetFlowBtns(int sheetId)
        {
            var btns=await (Manager as FlowSheetManager).GetFlowBtns(sheetId);
            return btns.Select(o => new
            {
                o.ButtonKey,
                o.ButtonName,
                o.ButtonClass,
                o.ConfirmMsg
            });
        }
    }
}
