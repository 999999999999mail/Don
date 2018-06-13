using Don.Common;
using Don.Common.Messages;
using Don.Infrastructure.Extensions;
using Don.Infrastructure.Jwt;
using Don.Infrastructure.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Don.Common.Filters;
using Microsoft.EntityFrameworkCore;
using Don.Model.Entities;
using Don.Common.DTO;
using Don.Service.Interf;

namespace Don.Web.API.Controllers
{
    /// <summary>
    /// 用户 Controller 错误代码 10000+
    /// </summary>
    [Produces("application/json")]
    [Route("api/User")]
    [Authorize, TokenSession]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly JwtOptions _jwtOptions;

        private readonly IRedisClient _redisClient;

        public UserController(IUserService userService, IOptions<JwtOptions> jwtConf, IRedisClient redisClient)
        {
            _userService = userService;
            _jwtOptions = jwtConf.Value;
            _redisClient = redisClient;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Regist")]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<LoginResp> Regist([FromBody] RegistReq req)
        {
            var result = await _userService.Regist(req.LoginName, req.Password, req.Nickname, req.RealName, req.Tel, req.Email, req.RefCode, Request.GetClientIP(), req.ClientType, req.ClientId);
            if (result.NonzeroCode)
            {
                return (new LoginResp()).Fail(result.Msg);
            }

            return await Login(req.LoginName, req.Password, req.AuthCode, req.ClientType, req.ClientId, true);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<LoginResp> Login([FromBody] LoginReq req)
        {
            return await Login(req.LoginName, req.Password, req.AuthCode, req.ClientType, req.ClientId);
        }

        private async Task<LoginResp> Login(string loginName, string password, string authCode, byte clientType, string clientId, bool regist = false)
        {
            var resp = new LoginResp();

            var ip = Request.GetClientIP();
            var website = Request.GetReferer();

            var result = await _userService.Login(loginName, password, ip, website, clientType, clientId);
            if (result.NonzeroCode)
            {
                return resp.Fail(result.Msg);
            }

            // 生成本次登录的标识
            string loginId = $"__{ip}_{DateTime.Now.ToString("yyMMddHHmmssfffffff")}";

            // 生成 Token
            var token = new JwtTokenBuilder()
            .AddSecurityKey(JwtSecurityKey.Create(_jwtOptions.Secret))
            .AddIssuer(_jwtOptions.Issuer)
            .AddAudience(_jwtOptions.Audience)
            .AddClaim(Constants.LOGINNAME, loginName)
            .AddClaim(Constants.LOGINID, loginId)
            .AddExpiry(_jwtOptions.Expiry)
            .Build();

            // 写入缓存，以便 TokenSessionFilter使用
            await _redisClient.GetDatabase().StringSetAsync(loginName, loginId, TimeSpan.FromMinutes(_jwtOptions.Expiry));

            resp.AccessToken = token.Value;
            resp.TokenType = "Bearer";
            return resp;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<ResponseBase> Logout()
        {
            var resp = new ResponseBase();
            var loginName = HttpContext?.User?.FindFirst(p => p.Type == Constants.LOGINNAME)?.Value;
            if (string.IsNullOrEmpty(loginName))
            {
                return resp.Fail("未获取到登录信息");
            }
            await _redisClient.GetDatabase().KeyDeleteAsync(loginName);
            return resp;
        }
    }
}