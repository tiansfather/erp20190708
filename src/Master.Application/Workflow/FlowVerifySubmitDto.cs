﻿using Master.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WorkFlow
{
    /// <summary>
    /// 流程节点审核提交
    /// </summary>
    public class FlowVerifySubmitDto
    {
        public int FlowInstanceId { get; set; }
        /// <summary>
        /// 1:同意；2：不同意；3：驳回
        /// </summary>
        public TagState VerificationFinally { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string VerificationOpinion { get; set; }

        /// <summary>
        /// 驳回的步骤，即驳回到的节点ID
        /// </summary>
        public string NodeRejectStep { get; set; }
    }
}
