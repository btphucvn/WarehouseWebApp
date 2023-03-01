using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Inventory
    {
        public int InventoryId { get; set; }
        public int? YearPeriod { get; set; }
        public int? Year { get; set; }
        public int? Period { get; set; }
        public int? UnitId { get; set; }
        public int? QuantityFirst { get; set; }
        public int? QuantityInput { get; set; }
        public int? QuantityOutput { get; set; }
        public int? QuantityLast { get; set; }
        public int? TotalPrice { get; set; }
        public DateTime? DateCalculate { get; set; }

        public virtual Unit? Unit { get; set; }
    }
}
