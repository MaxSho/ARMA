using System;
using System.Collections.Generic;

namespace DesARMA.Models
{
    public partial class MainConfig
    {
        public string? Control { get; set; }
        public string? Shema { get; set; }
        public string? Folder { get; set; }
        public string NumbInput { get; set; } = null!;
    }
}
