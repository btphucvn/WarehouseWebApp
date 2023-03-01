using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Good
    {
        public Good()
        {
            Documentdetails = new HashSet<Documentdetail>();
        }

        public string Barcode { get; set; } = null!;
        public string? CategoryName { get; set; }
        public string? CategoryShortName { get; set; }
        public int? UnitId { get; set; }
        public float? Price { get; set; }
        public int? SupplierId { get; set; }
        public int? OriginId { get; set; }
        public int? GroupGoodId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? Disabled { get; set; }

        public virtual Groupgood? GroupGood { get; set; }
        public virtual Origin? Origin { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual Unit? Unit { get; set; }
        public virtual ICollection<Documentdetail> Documentdetails { get; set; }
    }
}
