using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Document
    {
        public Document()
        {
            Documentdetails = new HashSet<Documentdetail>();
        }

        public int DocumentId { get; set; }
        public string? DocumentCode { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? DateDocument { get; set; }
        public string? DocumentNumber2 { get; set; }
        public DateTime? DateDocument2 { get; set; }
        public int? Quantity { get; set; }
        public float? TotalAmount { get; set; }
        public string? Note { get; set; }
        public int? CompanyId { get; set; }
        public int? UnitId { get; set; }
        public int? UnitOut { get; set; }
        public int? SupplierId { get; set; }
        public bool? Status { get; set; }
        public int? DocumentType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? DeletedBy { get; set; }

        public virtual Company? Company { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual Unit? Unit { get; set; }
        public virtual ICollection<Documentdetail> Documentdetails { get; set; }
    }
}
