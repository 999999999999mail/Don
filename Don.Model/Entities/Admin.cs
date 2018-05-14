using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Entities
{
    public class Admin
    {
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string Password { get; set; }

        public string RealName { get; set; }

        public string Tel { get; set; }

        public string EMail { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 状态 0：正常，1：禁用
        /// </summary>
        public byte State { get; set; }
    }
}
