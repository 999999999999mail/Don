using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Entities
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }
        /// <summary>
        /// 状态 0：正常，1：禁用
        /// </summary>
        public byte State { get; set; }

        public DateTime CreateTime { get; set; }

        public bool Sys { get; set; }
    }
}
