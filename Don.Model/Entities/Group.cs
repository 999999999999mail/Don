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

        public bool Deleted { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
