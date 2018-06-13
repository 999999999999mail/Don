using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Common.Enums
{
    /// <summary>
    /// 权限
    /// </summary>
    public enum Permissions
    {
        /// <summary>
        /// 查看
        /// </summary>
        View = 1,
        /// <summary>
        /// 增加
        /// </summary>
        Add = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 4,
        /// <summary>
        /// 更改
        /// </summary>
        Update = 8
    }
}
