using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Entities;

namespace Context.context;

public partial class NaviMarkContext : DbContext,IContext
{
    public NaviMarkContext()
    {
    }

    public NaviMarkContext(DbContextOptions<NaviMarkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductStore> ProductStores { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NaviMark;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerName).HasMaxLength(20);
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .HasColumnName("email");
            entity.Property(e => e.Passward)
                .HasMaxLength(12)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("passward");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductName).HasMaxLength(20);
        });

        modelBuilder.Entity<ProductStore>(entity =>
        {
            entity.ToTable("ProductStore");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductStores).HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.Store).WithMany(p => p.ProductStores).HasForeignKey(d => d.StoreId);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.Property(e => e.StoreName).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
