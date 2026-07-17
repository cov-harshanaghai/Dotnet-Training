using Ecommerce_DBFirst.Dtos;
using Ecommerce_DBFirst.Models;

namespace Ecommerce_DBFirst.Services
{
    public interface IProductService
    {
       Task< List<Product> > GetAllProductsAsync(ProductFilter filter);
        Task<List<Product>> SearchProductsAsync(string searchTerm);
        Task<Product?> GetProductByIdAsync(int id);
        Task<bool> ProductNameExistsAsync(string productName);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}