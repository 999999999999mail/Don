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
        private readonly IUnitOfWork _unitOfWork;

        private readonly JwtOptions _jwtOptions;

        private readonly IRedisClient _redisClient;

        public UserController(IUnitOfWork unitOfWork, IOptions<JwtOptions> jwtConf, IRedisClient redisClient)
        {
            _unitOfWork = unitOfWork;
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
            var resp = new LoginResp();

            var userRepo = _unitOfWork.GetRepository<User>();
            var user = await userRepo.GetFirstOrDefaultAsync(p => new { p.Id }, p => p.LoginName == req.LoginName);
            if (user != null)
            {
                return resp.Fail(10002, "登录名已被使用");
            }

            var defaultGroup = await _unitOfWork.GetRepository<Group>().GetFirstOrDefaultAsync(predicate: p => p.Sys);
            if (defaultGroup == null)
            {
                return resp.Fail(10001, "系统数据错误");
            }

            userRepo.Insert(new User
            {
                LoginName = req.LoginName,
                Password = req.Password.ToMD5(),
                Nickname = req.Nickname,
                RealName = req.RealName,
                Tel = req.Tel,
                EMail = req.EMail,
                RefCode = req.RefCode,
                RegTime = DateTime.Now,
                RegIP = Request.GetClientIP(),
                GroupId = defaultGroup.Id
            });
            await _unitOfWork.SaveChangesAsync();

            return await Login(req.LoginName, req.Password, req.AuthCode, req.ClientId, true);
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
            return await Login(req.LoginName, req.Password, req.AuthCode, req.ClientId);
        }

        private async Task<LoginResp> Login(string loginName, string password, string authCode, string clientId, bool regist = false)
        {
            var resp = new LoginResp();

            var pwdMD5 = password.ToMD5();
            var user = await _unitOfWork.GetRepository<User>()
                .GetFirstOrDefaultAsync(p => new UserLoginDto { UserId = p.Id }, predicate: p => p.LoginName == loginName && p.Password == pwdMD5);

            if (user == null)
            {
                return resp.Fail(11002, "登录名或密码错误");
            }

            var ip = Request.GetClientIP();

            // 写登录日志
            await _unitOfWork.GetRepository<UserLog>()
                .InsertAsync(new UserLog
                {
                    UserId = user.UserId,
                    Desc = "用户登录系统",
                    CreateTime = DateTime.Now,
                    Keywords = "Login",
                    IP = ip,
                    Website = Request.GetReferer(),
                });
            await _unitOfWork.SaveChangesAsync();

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
                return resp.Fail(12001, "未获取到登录信息");
            }
            await _redisClient.GetDatabase().KeyDeleteAsync(loginName);
            return resp;
        }
    }
}