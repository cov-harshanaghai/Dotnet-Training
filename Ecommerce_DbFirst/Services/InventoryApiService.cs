using Ecommerce_DBFirst.Models;
using Ecommerce_DBFirst.Controllers;

namespace Ecommerce_DBFirst.Services
{
    public class InventoryApiService : IInventoryApiService
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<InventoryApiService> _logger;

        public InventoryApiService(HttpClient httpClient , ILogger<InventoryApiService>logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<List<Inventory>> GetAllAsync()
        {
            try
            {
                var items = await _httpClient.GetFromJsonAsync<List<Inventory>>("api/Inventory");
                return items ?? new List<Inventory>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get the inventory list.");
                return new List<Inventory>();
            }
        }

        public async Task<Inventory> GetByProductIdAsync(int productId)
        {
            try
            {
                var res = await _httpClient.GetAsync($"api/Inventory/product/{productId}");
                if(res.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadFromJsonAsync<Inventory>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Failed to get the inventory for product id {productId}.");
                return null;

            }
        }
        public async Task<Inventory?> AddAsync(Inventory item)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Inventory", item);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Inventory>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to add inventory record for ProductId {ProductId}.", item.ProductId);
                return null;
            }
        }

        public async Task<bool> UpdateQuantityAsync(int productId, int newQuantity)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Inventory/product/{productId}", newQuantity);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to update quantity for ProductId {ProductId}.", productId);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int inventoryId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Inventory/{inventoryId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to delete inventory record {InventoryId}.", inventoryId);
                return false;
            }
        }
    }
}
