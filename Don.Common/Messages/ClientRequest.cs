using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    /// <summary>
    /// 需要带客户端信息的请求
    /// </summary>
    public class ClientRequest
    {
        /// <summary>
        /// 客户端类型 (1: PC, 2: Wap, 3: App)
        /// </summary>
        public byte ClientType { get; set; }
        /// <summary>
        /// 客户端Id (UUID)
        /// </summary>
        public string ClientId { get; set; }
    }
}
