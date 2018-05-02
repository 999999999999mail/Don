using Don.Common;
using Don.Common.Messages;
using Don.Infrastructure.Extensions;
using Don.Infrastructure.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Don.Web.API.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    [Authorize]
    public class UserController : Controller
    {
        private JwtOptions _jwtOptions;
        public UserController(IOptions<JwtOptions> jwtConf)
        {
            _jwtOptions = jwtConf.Value;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Regist")]
        [AllowAnonymous]
        public async Task<LoginResponse> Regist([FromBody] RegistRequest request)
        {
            var resp = new LoginResponse();


            return resp;
            
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<LoginResponse> Login([FromBody] LoginRequest request)
        {
            var resp = new LoginResponse();
            var ip = Request.GetClientIP();

            if (request.LoginName == "zhangsan" && request.Password == "123456")
            {
                var token = new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(_jwtOptions.Secret))
                .AddIssuer(_jwtOptions.Issuer)
                .AddAudience(_jwtOptions.Audience)
                .AddClaim(Constants.LOGINNAME, request.LoginName)
                .AddClaim(Constants.LOGINID, $"__{ip}_{DateTime.Now.ToString("yyMMddHHmmssfffffff")}")
                .AddExpiry(_jwtOptions.Expiry)
                .Build();

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
        public async Task<BaseResponse> Logout()
        {
            var resp = new BaseResponse();
            return resp;
        }
    }
}