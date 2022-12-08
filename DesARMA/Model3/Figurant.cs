using System;
using System.Collections.Generic;

namespace DesARMA.Model3
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
        public bool? ResFiz { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public bool? ResUr { get; set; }
        public bool? Status { get; set; }
        public bool? FCheck { get; set; }
        public string? Notes { get; set; }
        public DateTime? DtBirth { get; set; }

        public virtual Main? NumbInputNavigation { get; set; }
    }
}
