using FitnessFoodsLC.Context;
using FitnessFoodsLC.Domain.Products;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessFoodsLC.Interface.Context
{
    public interface IFitnessFoodsLCContext : IDbContext
    {
        public DbSet<Product> Products { get; }
    }
}
