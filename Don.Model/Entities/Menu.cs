using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Entities
{
    public class Menu
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public Menu Parent { get; set; }

        public string Url { get; set; }

        public int Order { get; set; }

        public string Css { get; set; }

        public int Permit { get; set; }
    }
}
