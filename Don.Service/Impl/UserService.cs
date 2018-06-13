using Don.Common.DTO;
using Don.Infrastructure.Extensions;
using Don.Model.Entities;
using Don.Service.Interf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Don.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="nickname"></param>
        /// <param name="realName"></param>
        /// <param name="tel"></param>
        /// <param name="email"></param>
        /// <param name="refCode"></param>
        /// <param name="ip"></param>
        /// <param name="clientType"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<BizResult<bool>> Regist(string loginName, string password, string nickname, string realName, string tel, string email, string refCode, string ip, byte clientType, string clientId)
        {
            var result = new BizResult<bool>();

            var userRepo = _unitOfWork.GetRepository<User>();
            var user = await userRepo.GetFirstOrDefaultAsync(p => new { p.Id }, p => p.LoginName == loginName);
            if (user != null)
            {
                return result.Fail("登录名已被使用");
            }

            var defaultGroup = await _unitOfWork.GetRepository<Group>().GetFirstOrDefaultAsync(predicate: p => p.Sys);
            if (defaultGroup == null)
            {
                return result.Fail("用户默认组未设置");
            }

            userRepo.Insert(new User
            {
                LoginName = loginName,
                Password = password.ToMD5(),
                Nickname = nickname,
                RealName = realName,
                Tel = tel,
                EMail = email,
                RefCode = refCode,
                RegTime = DateTime.Now,
                RegIP = ip,
                GroupId = defaultGroup.Id
            });

            await _unitOfWork.SaveChangesAsync();

            return result.Succeed(true);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="clientType"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<BizResult<UserLoginDTO>> Login(string loginName, string password, string ip, string website, byte clientType, string clientId)
        {
            var result = new BizResult<UserLoginDTO>();

            var pwdMD5 = password.ToMD5();
            var user = await _unitOfWork.GetRepository<User>()
                .GetFirstOrDefaultAsync(p => new UserLoginDTO { UserId = p.Id }, predicate: p => p.LoginName == loginName && p.Password == pwdMD5);
            if (user == null)
            {
                return result.Fail("登录名或密码错误");
            }

            // 写登录日志
            await _unitOfWork.GetRepository<UserLog>()
                .InsertAsync(new UserLog
                {
                    UserId = user.UserId,
                    Desc = "用户登录系统",
                    CreateTime = DateTime.Now,
                    Keywords = "Login",
                    IP = ip,
                    Website = website,
                });

            await _unitOfWork.SaveChangesAsync();

            return result.Succeed(user);
        }
    }
}
