using Don.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Don.Service.Interf
{
    public interface IUserService
    {
        Task<BizResult<bool>> Regist(string loginName, string password, string nickname, string realName, string tel, string email, string refCode, string ip, byte clientType, string clientId);
        Task<BizResult<UserLoginDTO>> Login(string loginName, string password, string ip, string website, byte clientType, string clientId);
    }
}
