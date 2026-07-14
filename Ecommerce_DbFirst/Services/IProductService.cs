using Ecommerce_DBFirst.Models;

namespace Ecommerce_DBFirst.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts(bool sortByPrice, int? categoryId);
        List<Product> SearchProducts(string searchTerm);
        Product? GetProductById(int id);
        bool ProductNameExists(string productName);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}