using DesARMA.Model3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesARMA
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = "";
        public string Hash { get; set; } = "";
        public List<Request> Requests { get; set; } = null!;

        
        public void IsCan(ModelContext modelContext, string numbInput) 
        {
           


            var blogs = from b in modelContext.Mains
                        where b.NumbInput.Equals(numbInput)
                        select b;



        }
    }
}
