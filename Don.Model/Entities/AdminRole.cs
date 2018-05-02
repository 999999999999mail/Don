using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Entities
{
    public class AdminRole
    {
        public int Id { get; set; }

        public int AdminId { get; set; }

        public Admin Admin { get; set; }
        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
