using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    public class LoginResp : ResponseBase
    {
        /// <summary>
        /// 授权 Token
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 授权 Token 类型
        /// </summary>
        public string TokenType { get; set; }
    }
}
