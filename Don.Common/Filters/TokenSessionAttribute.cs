using Don.Infrastructure.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don.Common.Filters
{
    /// <summary>
    /// Token Session 检查
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TokenSessionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Filters.Any(p => p is IAllowAnonymousFilter))
            {
                return;
            }
            if (context.HttpContext?.User?.Identity?.IsAuthenticated ?? false)
            {
                var loginName = context.HttpContext.User.FindFirst(c => c.Type == Constants.LOGINNAME)?.Value;
                var loginId = context.HttpContext.User.FindFirst(c => c.Type == Constants.LOGINID)?.Value;
                if (!string.IsNullOrEmpty(loginName) && !string.IsNullOrEmpty(loginId))
                {
                    var redisClient = context.HttpContext.RequestServices.GetRequiredService<IRedisClient>();
                    var cache = redisClient.GetDatabase();
                    var cacheValue = await cache.StringGetAsync(loginName);
                    if (cacheValue.HasValue && cacheValue == loginId)
                    {
                        IOptions<JwtOptions> jwtOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<JwtOptions>>();
                        await cache.KeyExpireAsync(loginName, TimeSpan.FromMinutes(jwtOptions.Value.Expiry));
                        return;
                    }
                }
                context.Result = new ChallengeResult();
            }
        }
    }
}
