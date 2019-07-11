using Master.Configuration;
using Master.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class WorkFlowDbContextFactory : DbContextFactory<WorkFlowDbContext>
    {

        public override WorkFlowDbContext CreateDbContext(DbContextOptions<WorkFlowDbContext> options)
        {
            return new WorkFlowDbContext(options);
        }
    }
}
