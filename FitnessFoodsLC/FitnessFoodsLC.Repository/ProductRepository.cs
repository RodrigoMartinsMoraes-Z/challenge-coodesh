using FitnessFoodsLC.Interface;
using FitnessFoodsLC.Interface.Context;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessFoodsLC.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IFitnessFoodsLCContext _context;

        public ProductRepository(IFitnessFoodsLCContext context)
        {
            _context = context;
        }

        public async Task<bool> Exist(string url)
        {
            return  _context.Products.Any(p => p.Url.Equals(url));
        }
    }
}
