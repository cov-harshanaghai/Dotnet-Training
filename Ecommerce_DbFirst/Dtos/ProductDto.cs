namespace Ecommerce_DBFirst.Dtos
{
    public class ProductDto
    {
    

        public string ProductName { get; set; } = null!;

        public int CategoryId { get; set; }

        public int StockQuantity { get; set; }
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}
