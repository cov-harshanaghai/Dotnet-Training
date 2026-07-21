using Ecommerce_DBFirst.Models;

namespace Ecommerce_DBFirst.Services
{
    public interface IInventoryApiService
    {
        Task<List<Inventory>> GetAllAsync();

        Task<Inventory?> GetByProductIdAsync(int productId);

        Task<Inventory?> AddAsync(Inventory item);

        Task<bool> UpdateQuantityAsync(int productId, int newQuantity);

        Task<bool> DeleteAsync(int inventoryId);
    }
}