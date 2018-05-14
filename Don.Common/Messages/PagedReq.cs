using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Messages
{
    public class PagedReq : RequestBase
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 0;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; } = 15;
        /// <summary>
        /// 排序
        /// </summary>
        public string OrderBy { get; set; }
    }
}
