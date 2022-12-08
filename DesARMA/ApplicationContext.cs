using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesARMA
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Request> Requests { get; set; } = null!;
        public DbSet<SRod> SRods { get; set; } = null!;
        public DbSet<SDav> SDavs { get; set; } = null!;
        public DbSet<FO> FOs { get; set; } = null!;
        public DbSet<UO> UOs { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=helloapp.db");
            optionsBuilder
               .UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=Request2Db;Trusted_Connection=True;"
                  // @"Data Source=(localdb)\v11.0;|DataDirectory|\RequestDb.mdf;"
                   );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            int i = 1;
            List<SRod> sRods = new List<SRod>();
            foreach (var s in Reest.sRodov)
            {
                sRods.Add(new SRod() { id=i++, name = s });
            }

            modelBuilder.Entity<SRod>().HasData(sRods);

            i = 1;
            List<SDav> sDav = new List<SDav>();
            foreach (var s in Reest.sDav)
            {
                sDav.Add(new SDav() { id = i++, name = s });
            }

            modelBuilder.Entity<SDav>().HasData(sDav);
            var r = new Request()
            {
                id = 1,
                pathDirectory = $"C:\\app\\99-99-992",
                numberIn = "99/99/992",
                count_Shemat = 0,
                typeOrgan = 1,
                isSubs = false,
                numberKP = "",
                dateIn = "",
                dateControl = "",
                numberRequest = "",
                dateRequest = null,
                vidOrg = "",
                addressOrg = "",
                positionSub = "",
                nameSub = "",
                numberOut = "",
                dateOut = null,
                co_executor = "",
                TEKA = "",
                article_CCU = "",
                note = "",
                typeAppea = 0,
                connectedPeople = ""
            };
            

            //List<Request> lr = new List<Request>();
            //lr.Add(r);

            //modelBuilder.Entity<Request>().HasData(lr);

            var u1 = new User()
            {
                Id = 1,
                Login = "max",
                Hash = "73627"
            };

            modelBuilder.Entity<User>().HasData(
                u1 ,
                new User() { Id = 2, Login = "max2", Hash = "1111"});



        }
    }


}
