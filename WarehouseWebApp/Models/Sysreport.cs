using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Sysreport
    {
        public int RepCode { get; set; }
        public string? Description { get; set; }
        public string? RepName { get; set; }
        public bool? Visibled { get; set; }
        public DateTime? FromDate { get; set; }
        public int? CompanyId { get; set; }
        public int? UnitId { get; set; }

        public virtual Company? Company { get; set; }
        public virtual Unit? Unit { get; set; }
    }
}
