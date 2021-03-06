﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    /// <summary>
    /// 需要带签名的请求
    /// </summary>
    public class SecureRequest
    {
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 请求时间戳
        /// </summary>
        public DateTime Time { get; set; }
    }
}
