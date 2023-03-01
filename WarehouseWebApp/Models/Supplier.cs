using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Documents = new HashSet<Document>();
            Goods = new HashSet<Good>();
        }

        public int SupplierId { get; set; }
        public string? SupplierCode { get; set; }
        public string? SupplierName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Good> Goods { get; set; }
    }
}
