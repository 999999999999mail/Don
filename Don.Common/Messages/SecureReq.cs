using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    public class SecureReq : RequestBase
    {
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 请求时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
