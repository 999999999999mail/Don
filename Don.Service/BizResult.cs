using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Service
{
    public class BizResult<T> : BizResultBase
    {
        public T Data { get; set; }

        public BizResult<T> Succeed(T data)
        {
            Data = data;
            Code = 0;
            return this;
        }
    }
}
