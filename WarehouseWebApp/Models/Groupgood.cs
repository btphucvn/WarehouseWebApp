using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Groupgood
    {
        public Groupgood()
        {
            Goods = new HashSet<Good>();
        }

        public int GroupGoodId { get; set; }
        public string? GroupName { get; set; }

        public virtual ICollection<Good> Goods { get; set; }
    }
}
