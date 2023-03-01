using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Sysfunc
    {
        public string? FuncCode { get; set; }
        public string? Sort { get; set; }
        public string? Description { get; set; }
        public bool? IsGroup { get; set; }
        public string? Parent { get; set; }
        public bool? Menu { get; set; }
        public string? Tips { get; set; }
    }
}
