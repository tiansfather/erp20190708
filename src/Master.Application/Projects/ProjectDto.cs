using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Projects
{
    [AutoMap(typeof(Project))]
    public class ProjectDto
    {
        public int Id { get; set; }
        public string ProjectSN { get; set; }
        public string ProjectName { get; set; }
        public int? ProjectPic { get; set; }
    }
}
