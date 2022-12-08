using System;
using System.Collections.Generic;

namespace DesARMA.Model3
{
    public partial class Final
    {
        public decimal? Id { get; set; }
        public string? LoginName { get; set; }
        public DateTime? DtInsert { get; set; }
        public long? Executor { get; set; }
        public string? NumbInput { get; set; }
        public string? NumbOut { get; set; }
        public DateTime? DtOut { get; set; }
        public DateTime? DtUpdate { get; set; }

        public virtual Main? NumbInputNavigation { get; set; }
    }
}
