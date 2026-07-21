using Ecommerce.Api.Models;

namespace Ecommerce.Api.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<Inventory>GetByProductIdAsync(int productId);

        Task<Inventory> AddAsync(Inventory item);
        Task<bool> UpdateQuantityAsync(int productId, int newQuantity);
        Task<bool> DeleteAsync(int inventoryId);
    }
}
