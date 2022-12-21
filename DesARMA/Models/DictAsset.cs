using System;
using System.Collections.Generic;

namespace DesARMA.Models
{
    public partial class DictAsset
    {
        public byte? CodeType { get; set; }
        public byte? Code { get; set; }
        public string? Name { get; set; }
        public byte? CodeAsset { get; set; }
        public DateTime? DtBegin { get; set; }
        public DateTime? DtEnd { get; set; }
    }
}
