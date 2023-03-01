using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Unit
    {
        public Unit()
        {
            Documents = new HashSet<Document>();
            Goods = new HashSet<Good>();
            Inventories = new HashSet<Inventory>();
            Sysreports = new HashSet<Sysreport>();
        }

        public int UnitId { get; set; }
        public int? CompanyId { get; set; }
        public string? UnitName { get; set; }
        public string? UnitCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
        public bool? Disabled { get; set; }
        public bool? Inventory { get; set; }

        public virtual Company? Company { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Good> Goods { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Sysreport> Sysreports { get; set; }
    }
}
