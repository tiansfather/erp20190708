using Abp.Application.Services;
using Abp.Authorization;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.DynamicScript
{
    [AbpAuthorize]
    public class ScriptAppService: ApplicationService
    {
        public virtual  async Task<string> TryCompile(ScriptCompileDto scriptCompileDto)
        {
            try
            {
                return ScriptRunner.CompileType(scriptCompileDto.Content).FullName;
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            
        }

    }
}
