using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WarehouseWebApp.Models
{
    public partial class dbwarehouseContext : DbContext
    {
        public dbwarehouseContext()
        {
        }

        public dbwarehouseContext(DbContextOptions<dbwarehouseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<Documentdetail> Documentdetails { get; set; } = null!;
        public virtual DbSet<Good> Goods { get; set; } = null!;
        public virtual DbSet<Groupgood> Groupgoods { get; set; } = null!;
        public virtual DbSet<Inventory> Inventories { get; set; } = null!;
        public virtual DbSet<Origin> Origins { get; set; } = null!;
        public virtual DbSet<Sequence> Sequences { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<Sysfunc> Sysfuncs { get; set; } = null!;
        public virtual DbSet<Sysreport> Sysreports { get; set; } = null!;
        public virtual DbSet<Sysright> Sysrights { get; set; } = null!;
        public virtual DbSet<Sysrightrep> Sysrightreps { get; set; } = null!;
        public virtual DbSet<Sysuser> Sysusers { get; set; } = null!;
        public virtual DbSet<Unit> Units { get; set; } = null!;
        public virtual DbSet<Unitcount> Unitcounts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=dbwarehouse;port=3306;user id=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.27-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_vietnamese_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("companies");

                entity.HasIndex(e => e.CompanyCode, "CompanyCode")
                    .IsUnique();

                entity.HasIndex(e => e.CompanyId, "CompanyID")
                    .IsUnique();

                entity.Property(e => e.CompanyId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CompanyID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CompanyCode).HasMaxLength(20);

                entity.Property(e => e.CompanyName).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("documents");

                entity.HasIndex(e => e.CompanyId, "fk_Documents_Companies");

                entity.HasIndex(e => e.SupplierId, "fk_Documents_Suppliers");

                entity.HasIndex(e => e.UnitId, "fk_Documents_Units");

                entity.Property(e => e.DocumentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("DocumentID");

                entity.Property(e => e.CompanyId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CompanyID");

                entity.Property(e => e.CreatedBy).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DateDocument).HasColumnType("datetime");

                entity.Property(e => e.DateDocument2).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasColumnType("int(11)");

                entity.Property(e => e.DocumentCode).HasMaxLength(20);

                entity.Property(e => e.DocumentNumber).HasMaxLength(20);

                entity.Property(e => e.DocumentNumber2).HasMaxLength(50);

                entity.Property(e => e.DocumentType).HasColumnType("int(11)");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.Quantity).HasColumnType("int(11)");

                entity.Property(e => e.SupplierId)
                    .HasColumnType("int(11)")
                    .HasColumnName("SupplierID");

                entity.Property(e => e.UnitId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UnitID");

                entity.Property(e => e.UnitOut)
                    .HasColumnType("int(11)")
                    .HasColumnName("Unit_Out");

                entity.Property(e => e.UpdatedBy).HasColumnType("int(11)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("fk_Documents_Companies");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("fk_Documents_Suppliers");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("fk_Documents_Units");
            });

            modelBuilder.Entity<Documentdetail>(entity =>
            {
                entity.ToTable("documentdetails");

                entity.HasIndex(e => e.DocumentId, "fk_DocumentDetails_Documents");

                entity.HasIndex(e => e.Barcode, "fk_DocumentDetails_Goods");

                entity.Property(e => e.DocumentDetailId)
                    .HasColumnType("int(11)")
                    .HasColumnName("DocumentDetailID");

                entity.Property(e => e.Barcode).HasMaxLength(13);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("DocumentID");

                entity.Property(e => e.Price).HasColumnType("int(11)");

                entity.Property(e => e.Quantity).HasColumnType("int(11)");

                entity.HasOne(d => d.BarcodeNavigation)
                    .WithMany(p => p.Documentdetails)
                    .HasForeignKey(d => d.Barcode)
                    .HasConstraintName("fk_DocumentDetails_Goods");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.Documentdetails)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("fk_DocumentDetails_Documents");
            });

            modelBuilder.Entity<Good>(entity =>
            {
                entity.HasKey(e => e.Barcode)
                    .HasName("PRIMARY");

                entity.ToTable("goods");

                entity.HasIndex(e => e.GroupGoodId, "fk_Goods_GroupGoods");

                entity.HasIndex(e => e.OriginId, "fk_Goods_Origins");

                entity.HasIndex(e => e.SupplierId, "fk_Goods_Suppliers");

                entity.HasIndex(e => e.UnitId, "fk_Goods_Units");

                entity.Property(e => e.Barcode).HasMaxLength(13);

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.CategoryShortName).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasColumnType("int(11)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GroupGoodId)
                    .HasColumnType("int(11)")
                    .HasColumnName("GroupGoodID");

                entity.Property(e => e.OriginId)
                    .HasColumnType("int(11)")
                    .HasColumnName("OriginID");

                entity.Property(e => e.SupplierId)
                    .HasColumnType("int(11)")
                    .HasColumnName("SupplierID");

                entity.Property(e => e.UnitId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UnitID");

                entity.HasOne(d => d.GroupGood)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.GroupGoodId)
                    .HasConstraintName("fk_Goods_GroupGoods");

                entity.HasOne(d => d.Origin)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.OriginId)
                    .HasConstraintName("fk_Goods_Origins");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("fk_Goods_Suppliers");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("fk_Goods_Units");
            });

            modelBuilder.Entity<Groupgood>(entity =>
            {
                entity.ToTable("groupgoods");

                entity.Property(e => e.GroupGoodId)
                    .HasColumnType("int(11)")
                    .HasColumnName("GroupGoodID");

                entity.Property(e => e.GroupName).HasMaxLength(50);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("inventories");

                entity.HasIndex(e => e.UnitId, "fk_Inventories_Units");

                entity.Property(e => e.InventoryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("InventoryID");

                entity.Property(e => e.DateCalculate).HasColumnType("datetime");

                entity.Property(e => e.Period).HasColumnType("int(11)");

                entity.Property(e => e.QuantityFirst).HasColumnType("int(11)");

                entity.Property(e => e.QuantityInput).HasColumnType("int(11)");

                entity.Property(e => e.QuantityLast).HasColumnType("int(11)");

                entity.Property(e => e.QuantityOutput).HasColumnType("int(11)");

                entity.Property(e => e.TotalPrice).HasColumnType("int(11)");

                entity.Property(e => e.UnitId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UnitID");

                entity.Property(e => e.Year).HasColumnType("int(11)");

                entity.Property(e => e.YearPeriod).HasColumnType("int(11)");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("fk_Inventories_Units");
            });

            modelBuilder.Entity<Origin>(entity =>
            {
                entity.ToTable("origins");

                entity.Property(e => e.OriginId)
                    .HasColumnType("int(11)")
                    .HasColumnName("OriginID");

                entity.Property(e => e.OriginName).HasMaxLength(100);
            });

            modelBuilder.Entity<Sequence>(entity =>
            {
                entity.HasKey(e => e.SeqName)
                    .HasName("PRIMARY");

                entity.ToTable("sequences");

                entity.Property(e => e.SeqName).HasMaxLength(50);

                entity.Property(e => e.SeqValue).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("suppliers");

                entity.Property(e => e.SupplierId)
                    .HasColumnType("int(11)")
                    .HasColumnName("SupplierID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.SupplierCode).HasMaxLength(50);

                entity.Property(e => e.SupplierName).HasMaxLength(100);
            });

            modelBuilder.Entity<Sysfunc>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sysfuncs");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.FuncCode).HasMaxLength(50);

                entity.Property(e => e.Parent).HasMaxLength(50);

                entity.Property(e => e.Sort).HasMaxLength(50);

                entity.Property(e => e.Tips).HasMaxLength(50);
            });

            modelBuilder.Entity<Sysreport>(entity =>
            {
                entity.HasKey(e => e.RepCode)
                    .HasName("PRIMARY");

                entity.ToTable("sysreports");

                entity.HasIndex(e => e.CompanyId, "fk_SysReports_Companies");

                entity.HasIndex(e => e.UnitId, "fk_SysReports_Units");

                entity.Property(e => e.RepCode).HasColumnType("int(11)");

                entity.Property(e => e.CompanyId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CompanyID");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.RepName).HasMaxLength(50);

                entity.Property(e => e.UnitId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UnitID");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Sysreports)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("fk_SysReports_Companies");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Sysreports)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("fk_SysReports_Units");
            });

            modelBuilder.Entity<Sysright>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sysrights");

                entity.Property(e => e.FuncCode).HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserID");

                entity.Property(e => e.UserRightId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserRightID");
            });

            modelBuilder.Entity<Sysrightrep>(entity =>
            {
                entity.HasKey(e => e.RepCode)
                    .HasName("PRIMARY");

                entity.ToTable("sysrightrep");

                entity.Property(e => e.RepCode).HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserID");

                entity.Property(e => e.UserRightId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserRightID");
            });

            modelBuilder.Entity<Sysuser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("sysusers");

                entity.HasIndex(e => e.CompanyId, "fk_SysUsers_Companies");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserID");

                entity.Property(e => e.CompanyId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CompanyID");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.LastPassChanged).HasMaxLength(50);

                entity.Property(e => e.PassWord).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Sysusers)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("fk_SysUsers_Companies");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("units");

                entity.HasIndex(e => e.UnitCode, "UnitCode")
                    .IsUnique();

                entity.HasIndex(e => e.UnitId, "UnitID")
                    .IsUnique();

                entity.HasIndex(e => e.CompanyId, "fk_Units_Companies");

                entity.Property(e => e.UnitId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UnitID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CompanyId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CompanyID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.UnitCode).HasMaxLength(20);

                entity.Property(e => e.UnitName).HasMaxLength(100);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("fk_Units_Companies");
            });

            modelBuilder.Entity<Unitcount>(entity =>
            {
                entity.ToTable("unitcounts");

                entity.Property(e => e.UnitCountId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UnitCountID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
