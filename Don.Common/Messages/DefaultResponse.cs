using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    public class DefaultResponse<T> : BaseResponse
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 设置返回数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public DefaultResponse<T> Succ(T data)
        {
            Data = data;
            Code = 0;
            Msg = "";
            return this;
        }
    }
}
