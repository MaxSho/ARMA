using DesARMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesARMA.Entities
{
    class RequestProgram
    {
        Main main = null!;
        ModelContext modelContext = null!;
        public RequestProgram(Main main, ModelContext modelContext)
        {
            this.main = main;
            this.modelContext = modelContext;
        }
        public List<Figurant> GetAllFigurants()
        {
            return (from b in modelContext.Figurants where b.NumbInput == main.NumbInput select b).ToList();
        }
    }
}
