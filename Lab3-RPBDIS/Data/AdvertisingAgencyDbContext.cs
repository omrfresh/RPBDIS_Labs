using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lab3_RPBDIS
{
    public partial class AdvertisingAgencyDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AdvertisingAgencyDbContext()
        {
            var builder = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public AdvertisingAgencyDbContext(DbContextOptions<AdvertisingAgencyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdType> AdTypes { get; set; }
        public virtual DbSet<AdditionalService> AdditionalServices { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderService> OrderServices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("RemoteSQLConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Cyrillic_General_CI_AS");

            modelBuilder.Entity<AdType>(entity =>
            {
                entity.HasKey(e => e.AdTypeId).HasName("PK__AdTypes__3B1564B99DFD6992");
                entity.Property(e => e.AdTypeId).HasColumnName("AdTypeID");
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<AdditionalService>(entity =>
            {
                entity.HasKey(e => e.ServiceId).HasName("PK__Addition__C51BB0EAABA96B2D");
                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
                entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A048078E5DD");
                entity.Property(e => e.ClientId).HasColumnName("ClientID");
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1020DA5F2");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Position).HasMaxLength(50);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA477A199CB2D");
                entity.Property(e => e.LocationId).HasColumnName("LocationID");
                entity.Property(e => e.AdDescription).HasMaxLength(200);
                entity.Property(e => e.AdTypeId).HasColumnName("AdTypeID");
                entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.LocationDescription).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.AdType).WithMany(p => p.Locations)
                    .HasForeignKey(d => d.AdTypeId)
                    .HasConstraintName("FK_Locations_AdTypes");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFE384A549");
                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.ClientId).HasColumnName("ClientID");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.LocationId).HasColumnName("LocationID");
                entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Orders_Clients");

                entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Orders_Employees");

                entity.HasOne(d => d.Location).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Orders_Locations");
            });

            modelBuilder.Entity<OrderService>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ServiceId }).HasName("PK__OrderSer__7FC1E0A1F7BD8C6A");
                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
                entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderServices)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderServices_Orders");

                entity.HasOne(d => d.Service).WithMany(p => p.OrderServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderServices_AdditionalServices");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
