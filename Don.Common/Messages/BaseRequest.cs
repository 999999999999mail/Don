using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    public class BaseRequest
    {
        /// <summary>
        /// 客户端 0: PC、1:Wap、2: AndroidApp、3: IPhoneApp（选填）
        /// </summary>
        public byte ClientType { get; set; }
    }
}
