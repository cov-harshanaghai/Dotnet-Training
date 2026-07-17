namespace Ecommerce_DBFirst.Dtos
{
    public enum ProductSort
    {
        None,
        Price,
        Name,
        Newest
    }
    public class ProductFilter
    {
        public int? CategoryId { get; set; }
       
        public ProductSort SortBy { get; set; }
     
    }
}
