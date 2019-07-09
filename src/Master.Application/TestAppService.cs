using Abp.Application.Services;
using Abp.Auditing;
using Abp.Web.Models;
using Master.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master
{
    public class TestAppService: MasterAppServiceBase<Project,int>
    {
        
        public string Get()
        {
            return "1";
        }
    }
}
