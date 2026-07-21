using Ecommerce.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Services
{
    public class InventoryService : IInventoryService
    {

        private readonly EcommerceContext _context;

        public InventoryService(EcommerceContext context)
        {
            _context = context;
        }
       
        public async Task<Inventory> AddAsync(Inventory item)
        {
            _context.Inventories.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(int inventoryId)
        {
          var item = _context.Inventories.FirstOrDefault(x => x.InventoryId == inventoryId);
            if(item == null)
            {
                return false;
            }
            _context.Inventories.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
                return await _context.Inventories.ToListAsync();
        }

        public async Task<Inventory> GetByProductIdAsync(int productId)
        {
           return await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
        }

        public async Task<bool> UpdateQuantityAsync(int productId, int newQuantity)
        {
            var item =await  _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
            if(item == null)
            {
                return false;
            }
            item.QuantityInStock = newQuantity;
            item.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
