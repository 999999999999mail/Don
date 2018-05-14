using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string Password { get; set; }

        public string Nickname { get; set; }

        public string RealName { get; set; }

        public string Tel { get; set; }

        public string EMail { get; set; }
        /// <summary>
        /// 状态 0：正常，1：禁用
        /// </summary>
        public byte State { get; set; }

        public bool Tester { get; set; }

        public string RegIP { get; set; }

        public DateTime RegTime { get; set; }
        /// <summary>
        /// 推荐码
        /// </summary>
        public string RefCode { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
