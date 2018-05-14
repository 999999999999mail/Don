using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Don.Common.Messages
{
    public class LoginReq : RequestBase
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "登录名不能为空")]
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
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
