using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Lab6.Models;
using System.Collections.Generic;

namespace Lab6.Data
{
    public class AdvertisingDbContext(DbContextOptions<AdvertisingDbContext> options) : DbContext(options)
    {

        public virtual DbSet<AdType> AdTypes { get; set; }
        public virtual DbSet<AdditionalService> AdditionalServices { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderService> OrderServices { get; set; }
    }
}
