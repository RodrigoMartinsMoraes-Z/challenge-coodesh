using FitnessFoodsLC.Domain.Products;
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

        public Task<bool> Exist(string url) => Task.FromResult(_context.Products.Any(p => p.Url.Contains(url)));

        public async Task Add(Product product)
        {
            await _context.Products.AddAsync(product);
            _context.SaveChanges();
        }

        public Task<Product?> FindByCode(long code) => Task.FromResult(_context.Products.FirstOrDefault(p => p.Code == code));

        public Task<IQueryable<Product>> GetList(int page = 1, int quantity = 100) => Task.FromResult(_context.Products.Skip((page -1) * quantity).Take(quantity));
    }
}
