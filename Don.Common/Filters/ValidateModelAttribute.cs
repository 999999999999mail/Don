using Don.Common.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don.Common.Filters
{
    /// <summary>
    /// 模型验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateModelAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid == false)
            {
                context.Result = new JsonResult(new ResponseBase
                {
                    Code = -403,
                    Msg = context.ModelState.Values.LastOrDefault()?.Errors?.LastOrDefault()?.ErrorMessage
                });
            }
            else
            {
                await next();
            }
        }
    }
}
