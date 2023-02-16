using System;
using System.Collections.Generic;

namespace DesARMA.Models
{
    public partial class Main
    {
        public Main()
        {
            FizUrs = new HashSet<FizUr>();
        }

        public decimal? Id { get; set; }
        public string? LoginName { get; set; }
        public DateTime? DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public DateTime? DtInput { get; set; }
        public string NumbInput { get; set; } = null!;
        public DateTime? DtOut { get; set; }
        public string? NumbOut { get; set; }
        public long? Executor { get; set; }
        public string? CoExecutor { get; set; }
        public long? Chief { get; set; }
        public string? Notes { get; set; }
        public string? NumbOutInit { get; set; }
        public DateTime? DtOutInit { get; set; }
        public byte? Agency { get; set; }
        public string? AgencyDep { get; set; }
        public string? Topic { get; set; }
        public string? ExecutorInit { get; set; }
        public DateTime? DtCheck { get; set; }
        public string? CpNumber { get; set; }
        public string? Folder { get; set; }
        public byte? RegId { get; set; }
        public string? Status { get; set; }
        public string? Art { get; set; }
        public string? Addr { get; set; }
        public string? Work { get; set; }
        public decimal? IdAcc { get; set; }
        public decimal? Id_id { get; set; }
        public virtual ICollection<FizUr> FizUrs { get; set; }
    }
}
