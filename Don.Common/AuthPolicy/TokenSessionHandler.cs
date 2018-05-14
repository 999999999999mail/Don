using Don.Infrastructure.Redis;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Don.Common.AuthPolicy
{
    public class TokenSessionHandler : AuthorizationHandler<TokenSessionRequirement>
    {
        private readonly IRedisClient _redisClient;

        public TokenSessionHandler(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenSessionRequirement requirement)
        {
            if (context.User != null)
            {
                var loginName = context.User.FindFirst(c => c.Type == Constants.LOGINNAME)?.Value;
                var loginId = context.User.FindFirst(c => c.Type == Constants.LOGINID)?.Value;
                if (!string.IsNullOrEmpty(loginName) && !string.IsNullOrEmpty(loginId))
                {
                    var cache = _redisClient.GetDatabase();
                    var cacheValue = await cache.StringGetAsync(loginName);
                    if (!string.IsNullOrEmpty(cacheValue) && cacheValue == loginId)
                    {
                        await cache.KeyExpireAsync(loginName, TimeSpan.FromMinutes(requirement.Expiry));
                        context.Succeed(requirement);
                    }
                    
                }
            }
        }
    }
}
