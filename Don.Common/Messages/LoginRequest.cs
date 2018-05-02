using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    public class LoginRequest : BaseRequest
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string AuthCode { get; set; }
    }
}
