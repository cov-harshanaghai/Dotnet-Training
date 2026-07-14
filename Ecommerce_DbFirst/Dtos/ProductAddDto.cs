namespace Ecommerce_DBFirst.Dtos
{
    public class ProductAddDto
    {
        public string ProductName { get; set; } = null!;

        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }

    }
}
