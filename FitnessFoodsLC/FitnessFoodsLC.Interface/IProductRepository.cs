using FitnessFoodsLC.Domain.Products;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessFoodsLC.Interface
{
    public interface IProductRepository
    {
        Task<bool> Exist(string url);
        Task Add(Product product);
        Task<Product?> FindByCode(long code);
        Task<IQueryable<Product>> GetList(int page = 1, int quantity = 100);
    }
}
