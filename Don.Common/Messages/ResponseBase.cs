﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    public class ResponseBase
    {
        /// <summary>
        /// 状态码 0 表示请求成功，其他表示有错误，原因请查看 Msg 说明
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息说明
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 设置错误说明
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public dynamic Fail(string msg)
        {
            Code = -1;
            Msg = msg;
            return this;
        }
        /// <summary>
        /// 设置错误码及说明
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
