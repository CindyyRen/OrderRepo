using Microsoft.EntityFrameworkCore; // ✅ 必须加
using onlineOrder.Models;
using System.Collections.Generic;

namespace onlineOrder.Data
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options)
    : base(options) // 必须把 options 传给 DbContext
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "手机", Description = "智能手机", Price = 2999, Stock = 10 },
                new Product { Id = 2, Name = "耳机", Description = "蓝牙耳机", Price = 199, Stock = 50 }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "Alice", Email = "alice@test.com" },
                new User { Id = 2, Username = "Bob", Email = "bob@test.com" }
            );

        }
    }
}
