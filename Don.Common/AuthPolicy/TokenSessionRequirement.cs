using Microsoft.AspNetCore.Authorization;

namespace Don.Common.AuthPolicy
{
    /// <summary>
    /// 检查 Token 缓存，控制同一时间只有一个 Token 存在，以及 Token 过期控制。
    /// </summary>
    public class TokenSessionRequirement : IAuthorizationRequirement
    {
        public int Expiry { get; set; }

        public TokenSessionRequirement()
            : this(60)
        {
        }

        public TokenSessionRequirement(int expiry)
        {
            Expiry = expiry;
        }
    }
}
