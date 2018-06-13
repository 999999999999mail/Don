using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Entities
{
    public class RolePermit
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public int MenuId { get; set; }

        public Menu Menu { get; set; }

        public int Permit { get; set; }
    }
}
