using Don.Common;
using Don.Common.Filters;
using Don.Common.Messages;
using Don.Infrastructure.Extensions;
using Don.Infrastructure.Jwt;
using Don.Infrastructure.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Don.Adm.API.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    [Authorize, TokenSession]
    public class UserController : Controller
    {
        private JwtOptions _jwtOptions;
        private readonly IRedisClient _redisClient;

        public UserController(IOptions<JwtOptions> jwtConf, IRedisClient redisClient)
        {
            _jwtOptions = jwtConf.Value;
            _redisClient = redisClient;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Regist")]
        [AllowAnonymous]
        public async Task<LoginResp> Regist([FromBody] RegistReq request)
        {
            var resp = new LoginResp();


            return resp;
            
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<LoginResp> Login([FromBody] LoginReq request)
        {
            var resp = new LoginResp();
            var ip = Request.GetClientIP();

            if (request.LoginName == "zhangsan" && request.Password == "123456")
            {
                string loginId = $"__{ip}_{DateTime.Now.ToString("yyMMddHHmmssfffffff")}";

                var token = new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(_jwtOptions.Secret))
                .AddIssuer(_jwtOptions.Issuer)
                .AddAudience(_jwtOptions.Audience)
                .AddClaim(Constants.LOGINNAME, request.LoginName)
                .AddClaim(Constants.LOGINID, loginId)
                .AddExpiry(_jwtOptions.Expiry)
                .Build();

                await _redisClient.GetDatabase().StringSetAsync(request.LoginName, loginId, TimeSpan.FromMinutes(_jwtOptions.Expiry));

                resp.AccessToken = token.Value;
                resp.TokenType = "Bearer";

                return resp;
            }
            else
            {
                return resp.Fail(401, "用户名或密码错误！");
            }
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<ResponseBase> Logout()
        {
            var resp = new ResponseBase();
            return resp;
        }
    }
}