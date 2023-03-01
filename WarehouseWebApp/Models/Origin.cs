using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Origin
    {
        public Origin()
        {
            Goods = new HashSet<Good>();
        }

        public int OriginId { get; set; }
        public string? OriginName { get; set; }

        public virtual ICollection<Good> Goods { get; set; }
    }
}
