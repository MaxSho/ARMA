﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DesARMA.Model3
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DictAgency> DictAgencies { get; set; } = null!;
        public virtual DbSet<DictAsset> DictAssets { get; set; } = null!;
        public virtual DbSet<DictAssetsType> DictAssetsTypes { get; set; } = null!;
        public virtual DbSet<DictCommon> DictCommons { get; set; } = null!;
        public virtual DbSet<DictReg> DictRegs { get; set; } = null!;
        public virtual DbSet<DictRequest> DictRequests { get; set; } = null!;
        public virtual DbSet<DictVob> DictVobs { get; set; } = null!;
        public virtual DbSet<Figurant> Figurants { get; set; } = null!;
        public virtual DbSet<Final> Finals { get; set; } = null!;
        public virtual DbSet<FizUr> FizUrs { get; set; } = null!;
        public virtual DbSet<Main> Mains { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<Stat> Stats { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 10.10.110.20)(PORT = 1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = arma)));Password=oracle;User ID=stat");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("STAT")
                .UseCollation("USING_NLS_COMP");

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

            modelBuilder.Entity<DictCommon>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DICT_COMMON");

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
                    .HasMaxLength(50)
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

                entity.ToTable("FIGURANT");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

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

                entity.Property(e => e.ResFiz)
                    .HasPrecision(1)
                    .HasColumnName("RES_FIZ");

                entity.Property(e => e.ResUr)
                    .HasPrecision(1)
                    .HasColumnName("RES_UR");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS");

                entity.Property(e => e.DtBirth)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_BIRTH");

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
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_INPUT");

                entity.Property(e => e.Addr)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("ADDR");

                entity.Property(e => e.Agency)
                    .HasPrecision(2)
                    .HasColumnName("AGENCY");

                entity.Property(e => e.AgencyDep)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("AGENCY_DEP");

                entity.Property(e => e.Art)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("ART");

                entity.Property(e => e.Chief)
                    .HasPrecision(13)
                    .HasColumnName("CHIEF");

                entity.Property(e => e.CoExecutor)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CO_EXECUTOR");

                entity.Property(e => e.CpNumber)
                    .HasMaxLength(500)
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
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EXECUTOR_INIT");

                entity.Property(e => e.Folder)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FOLDER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOGIN_NAME");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("NOTES");

                entity.Property(e => e.NumbOut)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_OUT");

                entity.Property(e => e.NumbOutInit)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NUMB_OUT_INIT");

                entity.Property(e => e.RegId)
                    .HasPrecision(2)
                    .HasColumnName("REG_ID");

                entity.Property(e => e.Status)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.Topic)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("TOPIC");

                entity.Property(e => e.Work)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("WORK");
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
                entity.HasNoKey();

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
                    .HasPrecision(1)
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