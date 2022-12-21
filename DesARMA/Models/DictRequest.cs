using System;
using System.Collections.Generic;

namespace DesARMA.Models
{
    public partial class DictRequest
    {
        public bool? Code { get; set; }
        public string? Name { get; set; }
        public DateTime? DtBegin { get; set; }
        public DateTime? DtEnd { get; set; }
    }
}
