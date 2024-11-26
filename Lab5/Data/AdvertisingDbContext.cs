//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Lab4.Models;

//namespace Lab4.Data
//{
//    public class AdvertisingDbContext(DbContextOptions<AdvertisingDbContext> options) : DbContext(options)
//    {

//        public virtual DbSet<AdType> AdTypes { get; set; }
//        public virtual DbSet<AdditionalService> AdditionalServices { get; set; }
//        public virtual DbSet<Client> Clients { get; set; }
//        public virtual DbSet<Employee> Employees { get; set; }
//        public virtual DbSet<Location> Locations { get; set; }
//        public virtual DbSet<Order> Orders { get; set; }
//        public virtual DbSet<OrderService> OrderServices { get; set; }
//    }
//}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;
using Microsoft.AspNetCore.Identity;

namespace Lab4.Data
{
    public class AdvertisingDbContext : IdentityDbContext<IdentityUser>
    {
        public AdvertisingDbContext(DbContextOptions<AdvertisingDbContext> options) : base(options)
        {
        }

        public virtual DbSet<AdType> AdTypes { get; set; }
        public virtual DbSet<AdditionalService> AdditionalServices { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderService> OrderServices { get; set; }
    }
}