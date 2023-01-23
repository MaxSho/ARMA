using System;
using System.Collections.Generic;

namespace DesARMA.Models
{
    public partial class Figurant
    {
        public decimal Id { get; set; }
        public string? LoginName { get; set; }
        public DateTime? DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public string? NumbInput { get; set; }
        public string? Fio { get; set; }
        public string? Ipn { get; set; }
        public int? ResFiz { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? ResUr { get; set; }
        public int? Status { get; set; }
        public bool? FCheck { get; set; }
        public string? Notes { get; set; }
        public DateTime? DtBirth { get; set; }
        public string? Shema { get; set; }
        public string? Control { get; set; }
        public virtual Main? NumbInputNavigation { get; set; }
    }
}
