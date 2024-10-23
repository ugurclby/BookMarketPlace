using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMarketPlace.Services.Order.Domain;

namespace BookMarketPlace.Services.Order.Infrastructure
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options):base(options)
        {        
        }

        public DbSet<BookMarketPlace.Services.Order.Domain.OrderAggragate.Order> Orders { get; set; }
        public DbSet<BookMarketPlace.Services.Order.Domain.OrderAggragate.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookMarketPlace.Services.Order.Domain.OrderAggragate.Order>().ToTable("Orders");
            modelBuilder.Entity<BookMarketPlace.Services.Order.Domain.OrderAggragate.OrderItem>().ToTable("OrderItems");
            modelBuilder.Entity<BookMarketPlace.Services.Order.Domain.OrderAggragate.OrderItem>().Property(x=>x.Price).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<BookMarketPlace.Services.Order.Domain.OrderAggragate.Order>().OwnsOne(x => x.Adress).WithOwner();

            base.OnModelCreating(modelBuilder);
        }
    }
}
