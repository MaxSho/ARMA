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
        public virtual DbSet<InfShipBookN> InfShipBooksN { get; set; } = null!;
        public virtual DbSet<InfShipRegN> InfShipRegsN { get; set; } = null!;
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

            modelBuilder.Entity<InfShipBookN>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("INF_SHIP_BOOK_N");

                entity.Property(e => e.ApNumb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AP_NUMB");

                entity.Property(e => e.ApNumb2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AP_NUMB2");

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

                entity.Property(e => e.BuildNumb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BUILD_NUMB");

                entity.Property(e => e.BuildNumb2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BUILD_NUMB2");

                entity.Property(e => e.Cap)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CAP");

                entity.Property(e => e.CaseNumb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CASE_NUMB");

                entity.Property(e => e.CaseNumb2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CASE_NUMB2");

                entity.Property(e => e.Cntr)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CNTR");

                entity.Property(e => e.Crew)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREW");

                entity.Property(e => e.Distr)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DISTR");

                entity.Property(e => e.Distr2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DISTR2");

                entity.Property(e => e.DtBirth)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DT_BIRTH");

                entity.Property(e => e.DtConst)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DT_CONST");

                entity.Property(e => e.DtEnd)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DT_END");

                entity.Property(e => e.DtEndT)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DT_END_T");

                entity.Property(e => e.DtIss)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DT_ISS");

                entity.Property(e => e.DtReg)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DT_REG");

                entity.Property(e => e.DtRen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DT_REN");

                entity.Property(e => e.Edrpou)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EDRPOU");

                entity.Property(e => e.Edrpou2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EDRPOU2");

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
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("EXCLUS");

                entity.Property(e => e.FNumb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("F_NUMB");

                entity.Property(e => e.Funct)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("FUNCT");

                entity.Property(e => e.Height)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("HEIGHT");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Ipn)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IPN");

                entity.Property(e => e.Ipn2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IPN2");

                entity.Property(e => e.IssA)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ISS_A");

                entity.Property(e => e.Length)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LENGTH");

                entity.Property(e => e.Mat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MAT");

                entity.Property(e => e.Model)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODEL");

                entity.Property(e => e.NameO)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("NAME_O");

                entity.Property(e => e.NameO2)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NAME_O2");

                entity.Property(e => e.OwnerS)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OWNER_S");

                entity.Property(e => e.Pass)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("PASS");

                entity.Property(e => e.Pib)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("PIB");

                entity.Property(e => e.Pib2)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("PIB2");

                entity.Property(e => e.Place)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PLACE");

                entity.Property(e => e.Reg)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("REG");

                entity.Property(e => e.Reg2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("REG2");

                entity.Property(e => e.RegA)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("REG_A");

                entity.Property(e => e.RegANew)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("REG_A_NEW");

                entity.Property(e => e.Settl)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SETTL");

                entity.Property(e => e.Settl2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SETTL2");

                entity.Property(e => e.ShipNumb)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SHIP_NUMB");

                entity.Property(e => e.ShipNumbOld)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SHIP_NUMB_OLD");

                entity.Property(e => e.ShipTick)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SHIP_TICK");

                entity.Property(e => e.Speed)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SPEED");

                entity.Property(e => e.Street)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("STREET");

                entity.Property(e => e.Street2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("STREET2");

                entity.Property(e => e.TermChart)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TERM_CHART");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");

                entity.Property(e => e.TypeOld)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_OLD");

                entity.Property(e => e.Width)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("WIDTH");
            });

            modelBuilder.Entity<InfShipRegN>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("INF_SHIP_REG_N");

                entity.Property(e => e.AddrC)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("ADDR_C");

                entity.Property(e => e.AddrU)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("ADDR_U");

                entity.Property(e => e.Area)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("AREA");

                entity.Property(e => e.CEng)
                    .HasMaxLength(150)
                    .IsUnicode(false)
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CNTR");

                entity.Property(e => e.ConvInst)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("CONV_INST");

                entity.Property(e => e.Cpct)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("CPCT");

                entity.Property(e => e.Crew)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREW");

                entity.Property(e => e.Ddw)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DDW");

                entity.Property(e => e.DocReg)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("DOC_REG");

                entity.Property(e => e.DtBirth)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DT_BIRTH");

                entity.Property(e => e.DtBirthC)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DT_BIRTH_C");

                entity.Property(e => e.DtCons)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DT_CONS");

                entity.Property(e => e.DtConst)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DT_CONST");

                entity.Property(e => e.DtFReg)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DT_F_REG");

                entity.Property(e => e.DtReg)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DT_REG");

                entity.Property(e => e.DtTmp)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DT_TMP");

                entity.Property(e => e.Edrpou)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("EDRPOU");

                entity.Property(e => e.EdrpouC)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EDRPOU_C");

                entity.Property(e => e.Eni)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ENI");

                entity.Property(e => e.Funct)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("FUNCT");

                entity.Property(e => e.Height)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("HEIGHT");

                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.IdHull)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ID_HULL");

                entity.Property(e => e.Imo)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("IMO");

                entity.Property(e => e.Inmar)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("INMAR");

                entity.Property(e => e.Ipn)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IPN");

                entity.Property(e => e.IpnC)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IPN_C");

                entity.Property(e => e.Length)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("LENGTH");

                entity.Property(e => e.Masts)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MASTS");

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

                entity.Property(e => e.NameOld)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME_OLD");

                entity.Property(e => e.PEng)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("P_ENG");

                entity.Property(e => e.Pass)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("PASS");

                entity.Property(e => e.PassC)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("PASS_C");

                entity.Property(e => e.Passen)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PASSEN");

                entity.Property(e => e.Pib)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PIB");

                entity.Property(e => e.PibC)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("PIB_C");

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
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("WIDTH");

                entity.Property(e => e.Yard)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("YARD");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
