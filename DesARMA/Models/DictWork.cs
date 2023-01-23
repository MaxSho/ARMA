using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesARMA.Models
{
    public partial class DictWork
    {
        public decimal? Id { get; set; }
        public decimal? Status { get; set; }
        public string Name { get; set; } = null!;
        public string NameMain { get; set; } = null!;
        public string? Addr { get; set; }
    }
}
