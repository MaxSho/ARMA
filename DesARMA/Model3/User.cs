using System;
using System.Collections.Generic;

namespace DesARMA.Model3
{
    public partial class User
    {
        public long? IdUser { get; set; }
        public string? LoginName { get; set; }
        public string? Name { get; set; }
        public byte? Depart { get; set; }
        public byte? Division { get; set; }
        public bool? Employee { get; set; }
        public bool? UserRole { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Active { get; set; }
        public string? Fio { get; set; }
        public bool? App105 { get; set; }
    }
}
