using System;
using System.Collections.Generic;

namespace DesARMA.Models
{
    public partial class Stat
    {
        public decimal? Id { get; set; }
        public string? LoginName { get; set; }
        public DateTime? DtInsert { get; set; }
        public string? NumbInput { get; set; }
        public string? CpNumber { get; set; }
        public byte? CodeAsset { get; set; }
        public long? Cnt { get; set; }
        public DateTime? DtUpdate { get; set; }
        public long? Executor { get; set; }
        public decimal? Source { get; set; }
        public string? Note { get; set; }

        public virtual Main? NumbInputNavigation { get; set; }
    }
}
