using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    public class RegistRequest : BaseRequest
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
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string EMail { get; set; }
        /// <summary>
        /// 推荐码( 选填 )
        /// </summary>
        public string RefCode { get; set; }
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
