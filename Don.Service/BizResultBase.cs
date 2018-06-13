using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Service
{
    /// <summary>
    /// 业务结果基类
    /// </summary>
    public abstract class BizResultBase
    {
        /// <summary>
        /// 返回代码: 0（零）表示成功，非零表示执行过程错误。
        /// </summary>
        public int Code { get; set; } = -1;
        /// <summary>
        /// Code 不等于 0
        /// </summary>
        public bool NonzeroCode
        {
            get
            {
                return Code != 0;
            }
        }
        /// <summary>
        /// 消息说明
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 设置返回错误
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public dynamic Fail(string msg)
        {
            Msg = msg;
            return this;
        }
        /// <summary>
        /// 设置返回错误
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public dynamic Fail(int code, string msg)
        {
            Code = code;
            Msg = msg;
            return this;
        }
    }
}
