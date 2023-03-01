using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Sysuser
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? PassWord { get; set; }
        public string? FullName { get; set; }
        public int? CompanyId { get; set; }
        public string? LastPassChanged { get; set; }
        public bool? Disabled { get; set; }
        public bool? IsGroup { get; set; }

        public virtual Company? Company { get; set; }
    }
}
