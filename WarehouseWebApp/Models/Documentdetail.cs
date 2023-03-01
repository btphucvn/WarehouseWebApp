using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Documentdetail
    {
        public int DocumentDetailId { get; set; }
        public int? DocumentId { get; set; }
        public string? Barcode { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }
        public float? TotalAmount { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Good? BarcodeNavigation { get; set; }
        public virtual Document? Document { get; set; }
    }
}
