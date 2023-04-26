using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using DesARMA.Models;
using System.Configuration;
using DesARMA.Registers.EDR;

namespace DesARMA.ModelCentextEDR
{
    public class ModelContextEDR : DbContext
    {
        private string strCon = null!;
        public ModelContextEDR()
        {
            this.strCon = GetStr();
        }
        public ModelContextEDR(DbContextOptions<ModelContext> options)
            : base(options)
        {
            this.strCon = GetStr();
        }
        private string GetStr()
        {
            string shif = ConfigurationManager.AppSettings["shedr"].ToString();
            List<byte> arrByteReturn = new List<byte>();
            List<byte> arrByteReturnDecrypt = new List<byte>();
            for (int i = 3; i <= shif.Length; i += 3)
            {
                var subStr = shif.Substring(i - 3, 3);
                arrByteReturn.Add(Convert.ToByte(subStr));
            }

            List<byte> key = new List<byte>();
            for (int i = arrByteReturn.Count - 8; i < arrByteReturn.Count; i++)
            {
                key.Add(arrByteReturn[i]);
            }

            for (int i = 0; i < arrByteReturn.Count - key.Count; i++)
            {
                arrByteReturnDecrypt.Add(Convert.ToByte(arrByteReturn[i] ^ key[i % key.Count]));
            }

            string result2 = System.Text.Encoding.UTF8.GetString(arrByteReturnDecrypt.ToArray());
            return result2;
        }
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Beneficiaries> Beneficiaries { get; set; } = null!;
        public virtual DbSet<Reason> Reasons { get; set; } = null!;
        public virtual DbSet<RelatedSubject> RelatedSubject { get; set; } = null!;
        public virtual DbSet<Branch> Branches { get; set; } = null!;
        public virtual DbSet<Head> Heads { get; set; } = null!;
        public virtual DbSet<Founder> Founders { get; set; } = null!;
        public virtual DbSet<ActivityKind> ActivityKinds { get; set; } = null!;
        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Contacts> Contacts { get; set; } = null!;
        public virtual DbSet<Inline_model> Inline_model { get; set; } = null!;
        public virtual DbSet<Inline_model_0> Inline_model_0 { get; set; } = null!;
        public virtual DbSet<Inline_model_1> Inline_model_1 { get; set; } = null!;
        public virtual DbSet<Inline_model_2> Inline_model_2 { get; set; } = null!;
        public virtual DbSet<Inline_model_3> Inline_model_3 { get; set; } = null!;
        public virtual DbSet<Inline_model_4> Inline_model_4 { get; set; } = null!;
        public virtual DbSet<Inline_model_5> Inline_model_5 { get; set; } = null!;
        public virtual DbSet<Inline_model_6> Inline_model_6 { get; set; } = null!;
        public virtual DbSet<Inline_model_7> Inline_model_7 { get; set; } = null!;
        public virtual DbSet<Inline_model_8> Inline_model_8 { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(this.strCon);
        }
    }
}
