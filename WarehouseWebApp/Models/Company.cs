using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Company
    {
        public Company()
        {
            Documents = new HashSet<Document>();
            Sysreports = new HashSet<Sysreport>();
            Sysusers = new HashSet<Sysuser>();
            Units = new HashSet<Unit>();
        }

        public int CompanyId { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
        public bool? Disabled { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Sysreport> Sysreports { get; set; }
        public virtual ICollection<Sysuser> Sysusers { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
    }
}
