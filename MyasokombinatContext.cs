using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Myasokombinat;

public partial class MyasokombinatContext : DbContext
{
    public MyasokombinatContext()
    {
    }

    public MyasokombinatContext(DbContextOptions<MyasokombinatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblManufacturer> TblManufacturers { get; set; }

    public virtual DbSet<TblOrderStatus> TblOrderStatuses { get; set; }

    public virtual DbSet<ProductName> ProductNames { get; set; }

    public virtual DbSet<TblProvider> TblProviders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.200.13;user=student;password=student;database=Myasokombinat", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.38-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("Order");

            entity.HasIndex(e => e.OrderStatus, "FK_Order_tbl_OrderStatus_OrderStatusId");

            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("OrderID");
            entity.Property(e => e.ClientFullName).HasMaxLength(255);
            entity.Property(e => e.OrderCode).HasColumnType("int(11)");
            entity.Property(e => e.OrderCompound).HasMaxLength(255);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.OrderNumber).HasMaxLength(255);
            entity.Property(e => e.OrderPickupPoint).HasColumnType("int(11)");
            entity.Property(e => e.OrderStatus).HasColumnType("int(11)");

            entity.HasOne(d => d.OrderStatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_tbl_OrderStatus_OrderStatusId");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductArticleNumber })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("OrderProduct");

            entity.HasIndex(e => e.ProductArticleNumber, "FK_OrderProduct_Product_ProductArticleNumber");

            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("OrderID");
            entity.Property(e => e.ProductArticleNumber)
                .HasMaxLength(100)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.Count).HasColumnType("int(11)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ProductArticleNumberNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductArticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductArticleNumber).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.HasIndex(e => e.ProductCategory, "FK_Product_tbl_Category_CategoryId");

            entity.HasIndex(e => e.ProductManufacturer, "FK_Product_tbl_Manufacturer_ManufacturerId");

            entity.HasIndex(e => e.ProductProvider, "FK_Product_tbl_Provider_ProviderId");

            entity.Property(e => e.ProductArticleNumber)
                .HasMaxLength(100)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.ProductCategory).HasColumnType("int(11)");
            entity.Property(e => e.ProductCost).HasColumnType("int(11)");
            entity.Property(e => e.ProductDescription).HasMaxLength(255);
            entity.Property(e => e.ProductDiscountAmount).HasColumnType("int(11)");
            entity.Property(e => e.ProductManufacturer).HasColumnType("int(11)");
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.ProductProvider).HasColumnType("int(11)");
            entity.Property(e => e.ProductQuantityInStock).HasColumnType("int(11)");
            entity.Property(e => e.ProductStatus).HasColumnType("int(11)");
            entity.Property(e => e.ProductUnit).HasPrecision(10);

            entity.HasOne(d => d.ProductCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_tbl_Category_CategoryId");

            entity.HasOne(d => d.ProductManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductManufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_tbl_Manufacturer_ManufacturerId");

            entity.HasOne(d => d.ProductProviderNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductProvider)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_tbl_Provider_ProviderId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .HasColumnType("int(11)")
                .HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("tbl_Category");

            entity.Property(e => e.CategoryId).HasColumnType("int(11)");
            entity.Property(e => e.CategoryTitle).HasMaxLength(255);
        });

        modelBuilder.Entity<TblManufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PRIMARY");

            entity.ToTable("tbl_Manufacturer");

            entity.Property(e => e.ManufacturerId).HasColumnType("int(11)");
            entity.Property(e => e.ManufacturerTitle).HasMaxLength(255);
        });

        modelBuilder.Entity<TblOrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("PRIMARY");

            entity.ToTable("tbl_OrderStatus");

            entity.Property(e => e.OrderStatusId).HasColumnType("int(11)");
            entity.Property(e => e.OrderStatusTitle).HasMaxLength(255);
        });
        ////Под вопросом
        modelBuilder.Entity<ProductName>(entity =>
        {
            entity.HasKey(e => e.ProductNameId).HasName("PRIMARY");

            entity.ToTable("tbl_ProductName");

            entity.Property(e => e.ProductNameId).HasColumnType("int(11)");
            entity.Property(e => e.ProductTitle).HasMaxLength(255);
        });
        ///Под вопросом
        modelBuilder.Entity<TblProvider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PRIMARY");

            entity.ToTable("tbl_Provider");

            entity.Property(e => e.ProviderId).HasColumnType("int(11)");
            entity.Property(e => e.ProviderTitle).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.UserRoleId, "FK_User_Role_RoleID");

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("UserID");
            entity.Property(e => e.UserLogin).HasColumnType("text");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserPassword).HasColumnType("text");
            entity.Property(e => e.UserPatronymic).HasMaxLength(100);
            entity.Property(e => e.UserRoleId)
                .HasColumnType("int(11)")
                .HasColumnName("UserRoleID");
            entity.Property(e => e.UserSurname).HasMaxLength(100);

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role_RoleID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
