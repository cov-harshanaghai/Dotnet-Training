using AutoMapper;
using Ecommerce_DBFirst.Dtos;
using Ecommerce_DBFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_DBFirst.Services
{
    public class ProductService : IProductService
    {
        private readonly EcommerceDbfirstContext _context;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;

        public ProductService(EcommerceDbfirstContext context, ILogger<ProductService> logger,IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<Product> > GetAllProductsAsync(ProductFilter filter)
        {
            _logger.LogInformation("Fetching all products from the database");
            var products = _context.Products.AsQueryable();
            if (filter.CategoryId.HasValue)
                products = products.Where(p => p.CategoryId == filter.CategoryId);
            switch (filter.SortBy)
            {
                case ProductSort.Price:
                    products = products.OrderBy(p => p.Price);
                    break;
                case ProductSort.Name:
                    products = products.OrderBy(p => p.ProductName);
                    break;
                case ProductSort.Newest:
                    products = products.OrderByDescending(p => p.ProductId); 
                    break;
                default:
                    break;
            }

           

            return await products.ToListAsync();
        }

        public async  Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                products = products.Where(p => p.ProductName.Contains(searchTerm));

            return await products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                _logger.LogWarning("Product with ID {ProductId} not found.", id);

            return product;
        }

        public async Task<bool> ProductNameExistsAsync(string productName)
        {
            return await _context.Products
                .AnyAsync(p => p.ProductName.ToLower() == productName.ToLower());
        }

        public async Task AddProductAsync(Product product)
        {
           await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Product {ProductName} saved with ID {ProductId}.",
                product.ProductName, product.ProductId);
        }

        public async Task UpdateProductAsync(Product updatedProduct)
        {
            var existingProduct =await _context.Products.FirstOrDefaultAsync(p => p.ProductId == updatedProduct.ProductId);

            if (existingProduct == null)
            {
                _logger.LogWarning("Attempted to update non-existent product with ID {ProductId}.", updatedProduct.ProductId);
                return;
            }

            //existingProduct.ProductName = updatedProduct.ProductName;
            //existingProduct.Price = updatedProduct.Price;
            //existingProduct.CategoryId = updatedProduct.CategoryId;
            //existingProduct.StockQuantity = updatedProduct.StockQuantity;
            //existingProduct.ImageUrl = updatedProduct.ImageUrl;
            _mapper.Map(updatedProduct, existingProduct);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Product {ProductId} updated.", existingProduct.ProductId);
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Product {ProductName} (ID {ProductId}) deleted.",
                product.ProductName, product.ProductId);
        }
    }
}