using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Entities
{
    /// <summary>
    /// 管理员操作日志
    /// </summary>
    public class AdminLog
    {
        public long Id { get; set; }

        public int AdminId { get; set; }

        public Admin Admin { get; set; }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 操作关键说明(关键字)
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// IP 地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 前端站点
        /// </summary>
        public string Website { get; set; }
    }
}
