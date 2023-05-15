using System;
using System.Collections.Generic;

namespace DesARMA.ModelRPS_SK_SR
{
    public partial class InfRegAvium
    {
        public decimal? Id { get; set; }
        public string? PlaneType { get; set; }
        public string? RegMark { get; set; }
        public string? SNumb { get; set; }
        public decimal? DtMan { get; set; }
        public decimal? MaxWeight { get; set; }
        public string? RegDoc { get; set; }
        public DateTime? DtReg { get; set; }
        public string? Operator { get; set; }
        public string? Owners { get; set; }
        public DateTime? DtInsert { get; set; }
        public DateTime? DtUpdate { get; set; }
        public decimal? Status { get; set; }
    }
}
