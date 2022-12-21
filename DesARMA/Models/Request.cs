using System;
using System.Collections.Generic;

namespace DesARMA.Models
{
    public partial class Request
    {
        public decimal? Id { get; set; }
        public string? LoginName { get; set; }
        public DateTime? DtInsert { get; set; }
        public long? Executor { get; set; }
        public string? NumbInput { get; set; }
        public string? NumbOutReq { get; set; }
        public DateTime? DtOutReq { get; set; }
        public string? Organ { get; set; }
        public bool? CodeRequest { get; set; }
        public DateTime? DtInputReq { get; set; }
        public string? NumbInputReq { get; set; }
        public DateTime? DtUpdate { get; set; }

        public virtual Main? NumbInputNavigation { get; set; }
    }
}
