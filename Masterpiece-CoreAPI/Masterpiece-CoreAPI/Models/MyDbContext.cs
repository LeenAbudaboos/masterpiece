using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece_CoreAPI.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<CartsProduct> CartsProducts { get; set; }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobApplication> JobApplications { get; set; }

    public virtual DbSet<PartnerJob> PartnerJobs { get; set; }

    public virtual DbSet<PartnerProduct> PartnerProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserService> UserServices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-QFTBA3H;Database=masterpiece;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE48837A9A6E4");

            entity.ToTable("Admin");

            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.Password).HasColumnName("password");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Carts__51BCD79710C034D5");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Carts__UserID__3E52440B");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B2A13911299");

            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems).HasForeignKey(d => d.CartId);

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__CartItems__Produ__440B1D61");
        });

        modelBuilder.Entity<CartsProduct>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__CartsPro__51BCD7972EDB9298");

            entity.ToTable("CartsProduct");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.CartsProducts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CartsProd__UserI__6FE99F9F");
        });

        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BFBDB26C0");

            entity.ToTable("Categorie");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__ContactU__C87C037C32CDA1F1");

            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__Jobs__056690E22504B3A4");

            entity.Property(e => e.JobId).HasColumnName("JobID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.JobImg).HasColumnName("jobImg");

            entity.HasOne(d => d.Category).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Jobs__CategoryID__46E78A0C");
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__JobAppli__C93A4F7978F97C9E");

            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.AppliedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.JobId).HasColumnName("JobID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Job).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK__JobApplic__JobID__4AB81AF0");

            entity.HasOne(d => d.User).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__JobApplic__UserI__4BAC3F29");
        });

        modelBuilder.Entity<PartnerJob>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__Partner___056690E2FB9946B2");

            entity.ToTable("Partner_Jobs");

            entity.Property(e => e.JobId).HasColumnName("JobID");
            entity.Property(e => e.DateSubmitted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.JobTitle).HasMaxLength(255);
        });

        modelBuilder.Entity<PartnerProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Partner___B40CC6EDB22FFB7D");

            entity.ToTable("Partner_Products");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.DateSubmitted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PartnerPhone).HasMaxLength(10);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED4F0ADE0C");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__398D8EEE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC1B7557FA");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<UserService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__UserServ__C51BB0EAD63DBB55");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.PostedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.UserServices)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__UserServi__Categ__4F7CD00D");

            entity.HasOne(d => d.PostedByNavigation).WithMany(p => p.UserServices)
                .HasForeignKey(d => d.PostedBy)
                .HasConstraintName("FK__UserServi__Poste__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
