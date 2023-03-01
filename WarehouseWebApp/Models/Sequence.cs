using System;
using System.Collections.Generic;

namespace WarehouseWebApp.Models
{
    public partial class Sequence
    {
        public string SeqName { get; set; } = null!;
        public int? SeqValue { get; set; }
    }
}
