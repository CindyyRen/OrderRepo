using Microsoft.EntityFrameworkCore; // ✅ 必须加
using onlineOrder.Models;
using System.Collections.Generic;

namespace onlineOrder.Data
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---- 1️⃣ Product 种子数据 ----
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "智能手机", Description = "5G 全面屏智能手机", Price = 2999, Stock = 10 },
                new Product { Id = 2, Name = "蓝牙耳机", Description = "降噪入耳式蓝牙耳机", Price = 199, Stock = 50 },
                new Product { Id = 3, Name = "笔记本电脑", Description = "轻薄便携办公本", Price = 6999, Stock = 15 },
                new Product { Id = 4, Name = "机械键盘", Description = "青轴背光机械键盘", Price = 399, Stock = 30 },
                new Product { Id = 5, Name = "无线鼠标", Description = "静音无线鼠标", Price = 99, Stock = 40 },
                new Product { Id = 6, Name = "显示器", Description = "27寸 2K IPS 显示器", Price = 1299, Stock = 20 },
                new Product { Id = 7, Name = "平板电脑", Description = "10.5英寸高清平板", Price = 2499, Stock = 12 },
                new Product { Id = 8, Name = "移动电源", Description = "10000mAh 快充移动电源", Price = 149, Stock = 60 },
                new Product { Id = 9, Name = "智能手表", Description = "运动健康监测智能手表", Price = 899, Stock = 25 },
                new Product { Id = 10, Name = "充电线", Description = "Type-C 快充数据线", Price = 29, Stock = 100 }
            );

            // ---- 2️⃣ User 种子数据 ----
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "Alice", Email = "alice@test.com" },
                new User { Id = 2, Username = "Bob", Email = "bob@test.com" },
                new User { Id = 3, Username = "Charlie", Email = "charlie@test.com" }
            );
            base.OnModelCreating(modelBuilder);


            // Product Price 指定精度
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // OrderItem UnitPrice 指定精度
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            // 配置一对多关系：Order ↔ OrderItem
            modelBuilder.Entity<Order>()
                //表示一个 Order 有多个 OrderItem。
                .HasMany(o => o.Items)
                //指定反向导航属性。意思是：“每个 OrderItem 都有一个对应的 Order”。
                .WithOne(oi => oi.Order)
                //定义了外键字段：在 OrderItem 表中，有一个 OrderId 列,用于指向它所属的 Order。
                .HasForeignKey(oi => oi.OrderId);
     

    }
}
}
