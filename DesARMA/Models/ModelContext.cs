using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NPOI.POIFS.FileSystem;

namespace DesARMA.Models
{
    public partial class ModelContext : DbContext
    {
        private string strCon = null!;
        public ModelContext()
        {
            this.strCon = GetStr();
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
            this.strCon = GetStr();
        }
        private string GetStr()
        {
            string shif = ConfigurationManager.AppSettings["sh"].ToString();
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
            //System.Windows.MessageBox.Show(result2);
            return result2;
        }

        public bool? isAuth;
        public virtual DbSet<DictAgWork> DictAgWorks { get; set; } = null!;
        public virtual DbSet<DictAgency> DictAgencies { get; set; } = null!;
        public virtual DbSet<DictAsset> DictAssets { get; set; } = null!;
        public virtual DbSet<DictAssetsType> DictAssetsTypes { get; set; } = null!;
        public virtual DbSet<DictBank> DictBanks { get; set; } = null!;
        public virtual DbSet<DictCommon> DictCommons { get; set; } = null!;
        public virtual DbSet<DictCommonOld> DictCommonOlds { get; set; } = null!;
        public virtual DbSet<DictReg> DictRegs { get; set; } = null!;
        public virtual DbSet<DictRequest> DictRequests { get; set; } = null!;
        public virtual DbSet<DictVob> DictVobs { get; set; } = null!;
        public virtual DbSet<Figurant> Figurants { get; set; } = null!;
        public virtual DbSet<Final> Finals { get; set; } = null!;
        public virtual DbSet<FizUr> FizUrs { get; set; } = null!;
        public virtual DbSet<Main> Mains { get; set; } = null!;
        public virtual DbSet<MainConfig> MainConfigs { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<Stat> Stats { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<DictWork> DictWorks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // local in config
                optionsBuilder.UseOracle(this.strCon);
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("STAT")
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<DictAgWork>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_AG_WORK");

                entity.Property(e => e.Addr)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("ADDR");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(130)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.Tel)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TEL");

                entity.Property(e => e.Url)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<DictAgency>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_AGENCY");

                entity.Property(e => e.Code)
                    .HasPrecision(2)
                    .HasColumnName("CODE");

                entity.Property(e => e.DtBegin)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_BEGIN");

                entity.Property(e => e.DtEnd)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_END");

                entity.Property(e => e.Name)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<DictAsset>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_ASSETS");

                entity.Property(e => e.Code)
                    .HasPrecision(2)
                    .HasColumnName("CODE");

                entity.Property(e => e.CodeAsset)
                    .HasPrecision(4)
                    .HasColumnName("CODE_ASSET");

                entity.Property(e => e.CodeType)
                    .HasPrecision(2)
                    .HasColumnName("CODE_TYPE");

                entity.Property(e => e.DtBegin)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_BEGIN");

                entity.Property(e => e.DtEnd)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_END");

                entity.Property(e => e.Name)
                    .HasMaxLength(51)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<DictAssetsType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_ASSETS_TYPE");

                entity.Property(e => e.Code)
                    .HasPrecision(2)
                    .HasColumnName("CODE");

                entity.Property(e => e.DtBegin)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_BEGIN");

                entity.Property(e => e.DtEnd)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_END");

                entity.Property(e => e.Name)
                    .HasMaxLength(62)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<DictBank>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_BANK");

                entity.Property(e => e.Adress)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("ADRESS");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FULLNAME");

                entity.Property(e => e.Glb)
                    .HasColumnType("NUMBER")
                    .HasColumnName("GLB");

                entity.Property(e => e.Ikod)
                    .HasColumnType("NUMBER")
                    .HasColumnName("IKOD");

                entity.Property(e => e.Kb)
                    .HasColumnType("NUMBER")
                    .HasColumnName("KB");

                entity.Property(e => e.Mfo)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MFO");

                entity.Property(e => e.Nb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NB");

                entity.Property(e => e.Np)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NP");

                entity.Property(e => e.Pi)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PI");

                entity.Property(e => e.Reestr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REESTR");

                entity.Property(e => e.Tb)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TB");
            });

            modelBuilder.Entity<DictCommon>(entity =>
            {
                entity.HasNoKey();

                //entity.HasKey(e => e.Id)
                //    .HasName("ID");

                entity.ToTable("DICT_COMMON");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.Domain)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DOMAIN");

                entity.Property(e => e.Id)
                    .HasPrecision(3)
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<DictCommonOld>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_COMMON_OLD");

                entity.Property(e => e.Code)
                    .HasPrecision(2)
                    .HasColumnName("CODE");

                entity.Property(e => e.Domain)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DOMAIN");

                entity.Property(e => e.Id)
                    .HasPrecision(3)
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<DictReg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_REG");

                entity.Property(e => e.DtBegin)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_BEGIN");

                entity.Property(e => e.DtEnd)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_END");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.RegId)
                    .HasPrecision(2)
                    .HasColumnName("REG_ID");
            });

            modelBuilder.Entity<DictRequest>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_REQUEST");

                entity.Property(e => e.Code)
                    .HasPrecision(1)
                    .HasColumnName("CODE");

                entity.Property(e => e.DtBegin)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_BEGIN");

                entity.Property(e => e.DtEnd)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_END");

                entity.Property(e => e.Name)
                    .HasMaxLength(21)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<DictVob>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_VOB");

                entity.HasIndex(e => e.VobId, "INDX_DIC_VOB_VOB_ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.VobId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("VOB_ID");
            });

            modelBuilder.Entity<Figurant>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => e.Id).HasName("ID");

                entity.ToTable("FIGURANT");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.DtBirth)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_BIRTH");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("DATE")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DT_INSERT");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_UPDATE");

                entity.Property(e => e.FCheck)
                    .HasPrecision(1)
                    .HasColumnName("F_CHECK");

                entity.Property(e => e.Fio)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FIO");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Ipn)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("IPN");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOGIN_NAME");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("NOTES");

                entity.Property(e => e.NumbInput)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_INPUT");

                entity.Property(e => e.Control)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CONTROL");

                entity.Property(e => e.Shema)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SHEMA");

                entity.Property(e => e.ResFiz)
                    .HasPrecision(1)
                    .HasColumnName("RES_FIZ");

                entity.Property(e => e.ResUr)
                    .HasPrecision(1)
                    .HasColumnName("RES_UR");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS");

                entity.HasOne(d => d.NumbInputNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.NumbInput)
                    .HasConstraintName("FK_FIGURANT_NUMB_INPUT");
            });

            modelBuilder.Entity<Final>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FINAL");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("DATE")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DT_INSERT");

                entity.Property(e => e.DtOut)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_OUT");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_UPDATE");

                entity.Property(e => e.Executor)
                    .HasPrecision(13)
                    .HasColumnName("EXECUTOR");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOGIN_NAME");

                entity.Property(e => e.NumbInput)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_INPUT");

                entity.Property(e => e.NumbOut)
                    .HasMaxLength(19)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_OUT");

                entity.HasOne(d => d.NumbInputNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.NumbInput)
                    .HasConstraintName("FK_FINAL_NUMB_INPUT");
            });

            modelBuilder.Entity<FizUr>(entity =>
            {
                entity.ToTable("FIZ_UR");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CntFiz)
                    .HasPrecision(3)
                    .HasColumnName("CNT_FIZ")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CntUr)
                    .HasPrecision(3)
                    .HasColumnName("CNT_UR")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CpNumber)
                    .HasPrecision(17)
                    .HasColumnName("CP_NUMBER");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("DATE")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DT_INSERT");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_UPDATE");

                entity.Property(e => e.Executor)
                    .HasPrecision(13)
                    .HasColumnName("EXECUTOR");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOGIN_NAME");

                entity.Property(e => e.NumbInput)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_INPUT");

                entity.HasOne(d => d.NumbInputNavigation)
                    .WithMany(p => p.FizUrs)
                    .HasForeignKey(d => d.NumbInput)
                    .HasConstraintName("FK_FIZ_UR_NUMB_INPUT");
            });

            modelBuilder.Entity<Main>(entity =>
            {
                entity.HasKey(e => e.NumbInput)
                    .HasName("MAIN_NUMB_INPUT_PK");

                entity.ToTable("MAIN");

                entity.Property(e => e.NumbInput)
                    //.HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_INPUT");

                entity.Property(e => e.Addr)
                    //.HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ADDR");

                entity.Property(e => e.Agency)
                    //.HasPrecision(2)
                    .HasColumnName("AGENCY");

                entity.Property(e => e.AgencyDep)
                    //.HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("AGENCY_DEP");

                entity.Property(e => e.Art)
                    //.HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("ART");

                entity.Property(e => e.Chief)
                    //.HasPrecision(13)
                    .HasColumnName("CHIEF");

                entity.Property(e => e.CoExecutor)
                    //.HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CO_EXECUTOR");

                entity.Property(e => e.CpNumber)
                    //.HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("CP_NUMBER");

                entity.Property(e => e.DtCheck)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_CHECK");

                entity.Property(e => e.DtInput)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_INPUT");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("DATE")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DT_INSERT");

                entity.Property(e => e.DtOut)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_OUT");

                entity.Property(e => e.DtOutInit)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_OUT_INIT");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_UPDATE");

                entity.Property(e => e.Executor)
                    .HasPrecision(13)
                    .HasColumnName("EXECUTOR");

                entity.Property(e => e.ExecutorInit)
                    //.HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EXECUTOR_INIT");

                entity.Property(e => e.Folder)
                    //.HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FOLDER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.IdAcc)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID_ACC");

                entity.Property(e => e.LoginName)
                    //.HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOGIN_NAME");

                entity.Property(e => e.Notes)
                    //.HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("NOTES");

                entity.Property(e => e.NumbOut)
                    //.HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_OUT");

                entity.Property(e => e.NumbOutInit)
                    //.HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_OUT_INIT");

                entity.Property(e => e.RegId)
                    .HasPrecision(2)
                    .HasColumnName("REG_ID");

                entity.Property(e => e.Status)
                    //.HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.Topic)
                    //.HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("TOPIC");

                entity.Property(e => e.Work)
                    //.HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("WORK");

                entity.Property(e => e.Id_id)
                   .HasColumnName("ID_ID");
            });

            modelBuilder.Entity<MainConfig>(entity =>
            {
               // entity.HasNoKey();

                entity.HasKey(e => e.NumbInput)
                    .HasName("NUMB_INPUT"); //UN_MAIN_CONFIG_NUMB_INPUT

                entity.ToTable("MAIN_CONFIG");

                //entity.HasIndex(e => e.NumbInput, "UN_MAIN_CONFIG_NUMB_INPUT")
                //    .IsUnique();

                entity.Property(e => e.Control)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CONTROL");

                entity.Property(e => e.Folder)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FOLDER");

                entity.Property(e => e.NumbInput)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_INPUT");

                entity.Property(e => e.Shema)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SHEMA");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("REQUEST");

                entity.Property(e => e.CodeRequest)
                    .HasPrecision(1)
                    .HasColumnName("CODE_REQUEST");

                entity.Property(e => e.DtInputReq)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_INPUT_REQ");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("DATE")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DT_INSERT");

                entity.Property(e => e.DtOutReq)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_OUT_REQ");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_UPDATE");

                entity.Property(e => e.Executor)
                    .HasPrecision(13)
                    .HasColumnName("EXECUTOR");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOGIN_NAME");

                entity.Property(e => e.NumbInput)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_INPUT");

                entity.Property(e => e.NumbInputReq)
                    .HasMaxLength(31)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_INPUT_REQ");

                entity.Property(e => e.NumbOutReq)
                    .HasMaxLength(21)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_OUT_REQ");

                entity.Property(e => e.Organ)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("ORGAN");

                entity.HasOne(d => d.NumbInputNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.NumbInput)
                    .HasConstraintName("FK_REQUEST_NUMB_INPUT");
            });

            modelBuilder.Entity<Stat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("STAT");

                entity.Property(e => e.Cnt)
                    .HasPrecision(11)
                    .HasColumnName("CNT");

                entity.Property(e => e.CodeAsset)
                    .HasPrecision(4)
                    .HasColumnName("CODE_ASSET");

                entity.Property(e => e.CpNumber)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("CP_NUMBER");

                entity.Property(e => e.DtInsert)
                    .HasColumnType("DATE")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DT_INSERT");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_UPDATE");

                entity.Property(e => e.Executor)
                    .HasPrecision(13)
                    .HasColumnName("EXECUTOR");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOGIN_NAME");

                entity.Property(e => e.Note)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTE");

                entity.Property(e => e.NumbInput)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_INPUT");

                entity.Property(e => e.Source)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SOURCE");

                entity.HasOne(d => d.NumbInputNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.NumbInput)
                    .HasConstraintName("FK_STAT_NUMB_INPUT");
            });

            modelBuilder.Entity<User>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => e.IdUser)
                    .HasName("ID_USER");

                entity.ToTable("USERS");

                entity.HasIndex(e => e.Email, "USERS_EMAIL_UK")
                    .IsUnique();

                entity.HasIndex(e => e.IdUser, "USERS_ID_USER_UK")
                    .IsUnique();

                entity.HasIndex(e => e.LoginName, "USERS_LOGIN_NAME_PK")
                    .IsUnique();

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVE")
                    .IsFixedLength();

                entity.Property(e => e.App105)
                    .HasPrecision(1)
                    .HasColumnName("APP_105");

                entity.Property(e => e.Depart)
                    .HasPrecision(2)
                    .HasColumnName("DEPART");

                entity.Property(e => e.Division)
                    .HasPrecision(2)
                    .HasColumnName("DIVISION");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Employee)
                    //.HasPrecision(1)
                    .HasColumnName("EMPLOYEE");

                entity.Property(e => e.Fio)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("FIO");

                entity.Property(e => e.IdUser)
                    .HasPrecision(13)
                    .HasColumnName("ID_USER");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("LOGIN_NAME");

                entity.Property(e => e.Name)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Password)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.UserRole)
                    .HasPrecision(1)
                    .HasColumnName("USER_ROLE");
            });

            modelBuilder.Entity<DictWork>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_WORK");

                entity.Property(e => e.Addr)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("ADDR");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(130)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.NameMain)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("NAME_MAIN");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

            });
            
            modelBuilder.HasSequence("SEQ_FIGURANT_ID");

            modelBuilder.HasSequence("SEQ_FINAL_ID");

            modelBuilder.HasSequence("SEQ_FIZ_UR_ID");

            modelBuilder.HasSequence("SEQ_MAIN_ID");

            modelBuilder.HasSequence("SEQ_REQUEST_ID");

            modelBuilder.HasSequence("SEQ_STAT_ID");

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
