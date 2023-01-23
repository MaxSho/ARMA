using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesARMA.Registers.EDR
{
    public class EDRClass
    {
        public string url { get; set; }
        public uint id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int state { get; set; }
        public string state_text { get; set; }
    }
}
