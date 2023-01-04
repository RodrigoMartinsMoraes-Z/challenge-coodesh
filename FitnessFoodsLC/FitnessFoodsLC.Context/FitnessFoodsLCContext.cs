using FitnessFoodsLC.Context.EntityTypeConfiguration;
using FitnessFoodsLC.Domain.Products;
using FitnessFoodsLC.Interface.Context;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessFoodsLC.Context
{
    public class FitnessFoodsLCContext : DbContext, IFitnessFoodsLCContext
    {
        public DbSet<Product> Products { get; set; }

        public FitnessFoodsLCContext(DbContextOptions<FitnessFoodsLCContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
        }
    }
}
