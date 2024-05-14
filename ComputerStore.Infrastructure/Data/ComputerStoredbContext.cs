using ComputerStore.Domain.Models;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Infrastructure.Data
{
    public class ComputerStoredbContext : DbContext
    {
        //builder.Services.AddDbContext<ComputerStoredbContext>(db => db.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        public ComputerStoredbContext(DbContextOptions<ComputerStoredbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategoryMap> productCategoryMaps { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
         .Property(p => p.Price)
         .HasColumnType("decimal(18,2)"); // Adjust precision and scale as per your requirements

            // Inserting record in OurHero table
            //modelBuilder.Entity<Product>().HasData(
            //    new Product
            //    {
            //        Id = 1,
            //        Name = "Intel's Core i9-9900K",
            //        //Categories = "1",
            //        Price = 20,
            //        Quantity = 1,
            //        Categories= 
            //    }
            //);

        }
   
}
}


