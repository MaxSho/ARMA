using System;
using System.Collections.Generic;

namespace DesARMA.Model3
{
    public partial class FizUr
    {
        public decimal Id { get; set; }
        public string? LoginName { get; set; }
        public DateTime? DtInsert { get; set; }
        public string? NumbInput { get; set; }
        public long? CpNumber { get; set; }
        public byte? CntFiz { get; set; }
        public byte? CntUr { get; set; }
        public DateTime? DtUpdate { get; set; }
        public long? Executor { get; set; }

        public virtual Main? NumbInputNavigation { get; set; }
    }
}
