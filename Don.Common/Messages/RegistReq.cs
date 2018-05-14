using System.ComponentModel.DataAnnotations;

namespace Don.Common.Messages
{
    public class RegistReq : RequestBase
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "登录名必填")]
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        public string Password { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string EMail { get; set; }
        /// <summary>
        /// 推荐码
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
