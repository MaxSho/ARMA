using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DesARMA.ModelRPS_SK_SR
{
    public partial class ModelContextRPS_SK_SR : DbContext
    {
        private string strCon = null!;
        public ModelContextRPS_SK_SR()
        {
            this.strCon = GetStr();
        }

        public ModelContextRPS_SK_SR(DbContextOptions<ModelContextRPS_SK_SR> options)
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
        public virtual DbSet<InfRegAvium> InfRegAvia { get; set; } = null!;
        public virtual DbSet<InfShipBook> InfShipBooks { get; set; } = null!;
        public virtual DbSet<InfShipReg> InfShipRegs { get; set; } = null!;
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle(strCon);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("STAT1")
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<InfRegAvium>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("INF_REG_AVIA");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_INSERT");

                entity.Property(e => e.DtMan)
                    .HasPrecision(4)
                    .HasColumnName("DT_MAN");

                entity.Property(e => e.DtReg)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_REG");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_UPDATE");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("ID");

                entity.Property(e => e.MaxWeight)
                    .HasPrecision(6)
                    .HasColumnName("MAX_WEIGHT");

                entity.Property(e => e.Operator)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("OPERATOR");

                entity.Property(e => e.Owners)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("OWNERS");

                entity.Property(e => e.PlaneType)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("PLANE_TYPE");

                entity.Property(e => e.RegDoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REG_DOC");

                entity.Property(e => e.RegMark)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REG_MARK");

                entity.Property(e => e.SNumb)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("S_NUMB");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1\n");
            });

            modelBuilder.Entity<InfShipBook>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("INF_SHIP_BOOK");

                entity.Property(e => e.BookNumb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BOOK_NUMB");

                entity.Property(e => e.BookSnumb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BOOK_SNUMB");

                entity.Property(e => e.Brand)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BRAND");

                entity.Property(e => e.DtConst)
                    .HasPrecision(5)
                    .HasColumnName("DT_CONST");

                entity.Property(e => e.DtEnd)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DT_END");

                entity.Property(e => e.EngNumb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ENG_NUMB");

                entity.Property(e => e.EngPower)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ENG_POWER");

                entity.Property(e => e.Engine)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ENGINE");

                entity.Property(e => e.Exclus)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EXCLUS");

                entity.Property(e => e.Funct)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("FUNCT");

                entity.Property(e => e.Height)
                    .HasColumnType("NUMBER(7,4)")
                    .HasColumnName("HEIGHT");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("ID");

                entity.Property(e => e.Length)
                    .HasColumnType("NUMBER(9,4)")
                    .HasColumnName("LENGTH");

                entity.Property(e => e.Mat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MAT");

                entity.Property(e => e.Owner)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OWNER");

                entity.Property(e => e.Pib)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("PIB");

                entity.Property(e => e.ShipNumb)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SHIP_NUMB");

                entity.Property(e => e.ShipTick)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SHIP_TICK");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");

                entity.Property(e => e.Width)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("WIDTH");
            });

            modelBuilder.Entity<InfShipReg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("INF_SHIP_REG");

                entity.Property(e => e.Area)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("AREA");

                entity.Property(e => e.CEng)
                    .HasPrecision(5)
                    .HasColumnName("C_ENG");

                entity.Property(e => e.Chart)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("CHART");

                entity.Property(e => e.Class)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CLASS");

                entity.Property(e => e.Cntr)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CNTR");

                entity.Property(e => e.Cpct)
                    .HasColumnType("NUMBER(9,4)")
                    .HasColumnName("CPCT");

                entity.Property(e => e.DtCons)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DT_CONS");

                entity.Property(e => e.DtFReg)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_F_REG");

                entity.Property(e => e.DtReg)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_REG");

                entity.Property(e => e.DtTmp)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DT_TMP");

                entity.Property(e => e.Funct)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FUNCT");

                entity.Property(e => e.Height)
                    .HasColumnType("NUMBER(9,4)")
                    .HasColumnName("HEIGHT");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("ID");

                entity.Property(e => e.IdHull)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID_HULL");

                entity.Property(e => e.Imo)
                    .HasPrecision(10)
                    .HasColumnName("IMO");

                entity.Property(e => e.Inmar)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("INMAR");

                entity.Property(e => e.Length)
                    .HasColumnType("NUMBER(9,4)")
                    .HasColumnName("LENGTH");

                entity.Property(e => e.Mat)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("MAT");

                entity.Property(e => e.Model)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODEL");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.NameL)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("NAME_L");

                entity.Property(e => e.PEng)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("P_ENG");

                entity.Property(e => e.Pib)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("PIB");

                entity.Property(e => e.Port)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PORT");

                entity.Property(e => e.RegNumb)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("REG_NUMB");

                entity.Property(e => e.TEng)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("T_ENG");

                entity.Property(e => e.TReg)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("T_REG");

                entity.Property(e => e.Type)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");

                entity.Property(e => e.Width)
                    .HasColumnType("NUMBER(9,4)")
                    .HasColumnName("WIDTH");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
