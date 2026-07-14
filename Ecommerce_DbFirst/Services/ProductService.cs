using Ecommerce_DBFirst.Models;

namespace Ecommerce_DBFirst.Services
{
    public class ProductService : IProductService
    {
        private readonly EcommerceDbfirstContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(EcommerceDbfirstContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Product> GetAllProducts(bool sortByPrice, int? categoryId)
        {
            _logger.LogInformation("Fetching all products from the database. SortByPrice: {SortByPrice}, CategoryId: {CategoryId}", sortByPrice, categoryId);
            var products = _context.Products.AsQueryable();

            if (sortByPrice)
                products = products.OrderBy(p => p.Price);

            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId);

            return products.ToList();
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                products = products.Where(p => p.ProductName.Contains(searchTerm));

            return products.ToList();
        }

        public Product? GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                _logger.LogWarning("Product with ID {ProductId} not found.", id);

            return product;
        }

        public bool ProductNameExists(string productName)
        {
            return _context.Products
                .Any(p => p.ProductName.ToLower() == productName.ToLower());
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            _logger.LogInformation("Product {ProductName} saved with ID {ProductId}.",
                product.ProductName, product.ProductId);
        }

        public void UpdateProduct(Product updatedProduct)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.ProductId == updatedProduct.ProductId);

            if (existingProduct == null)
            {
                _logger.LogWarning("Attempted to update non-existent product with ID {ProductId}.", updatedProduct.ProductId);
                return;
            }

            existingProduct.ProductName = updatedProduct.ProductName;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.CategoryId = updatedProduct.CategoryId;
            existingProduct.StockQuantity = updatedProduct.StockQuantity;
            existingProduct.ImageUrl = updatedProduct.ImageUrl;

            _context.SaveChanges();
            _logger.LogInformation("Product {ProductId} updated.", existingProduct.ProductId);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
            _logger.LogInformation("Product {ProductName} (ID {ProductId}) deleted.",
                product.ProductName, product.ProductId);
        }
    }
}