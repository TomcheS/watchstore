using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using watchstore.Models;

namespace watchstore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Watches> Watches { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-watchstore-A0E6F45C-1703-4D5B-AC4D-3988D30725B3;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
