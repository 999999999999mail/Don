using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Entities
{
    public class RoleMenu
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public int MenuId { get; set; }

        public Menu Menu { get; set; }
    }
}
