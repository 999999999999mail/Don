using Don.Common.Messages;
using Don.Infrastructure.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Don.Common.Middleware
{
    /// <summary>
    /// 请求处理中间件，包括 Token 有效性检查和访问记录跟踪等，请在验证中间件之后执行。
    /// </summary>
    public class VisitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRedisClient _redisClient;
        private JwtOptions _jwtOptions;

        public VisitMiddleware(RequestDelegate next, IRedisClient redisClient, IOptions<JwtOptions> options)
        {
            _next = next;
            _redisClient = redisClient;
            _jwtOptions = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            string loginName = string.Empty;
            string loginId = string.Empty;
            #region Token 有效性检查
            if (context.User?.Identity?.IsAuthenticated ?? false)
            {
                bool auth = false;
                loginName = context.User.FindFirst(c => c.Type == Constants.LOGINNAME)?.Value;
                loginId = context.User.FindFirst(c => c.Type == Constants.LOGINID)?.Value;
                if (!string.IsNullOrEmpty(loginName) && !string.IsNullOrEmpty(loginId))
                {
                    var cache = _redisClient.GetDatabase();
                    var cacheValue = await cache.StringGetAsync(loginName);
                    if (cacheValue.HasValue && cacheValue == loginId)
                    {
                        await cache.KeyExpireAsync(loginName, TimeSpan.FromMinutes(_jwtOptions.Expiry));
                        auth = true;
                    }
                }
                if (!auth)
                {
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new BaseResponse { Code = 401, Msg = "Token Invalid" },
                        Formatting.Indented,
                        new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }));
                    return;
                }
            }
            #endregion
            await _next(context);
            #region 访问记录跟踪

            #endregion
        }
    }
}
