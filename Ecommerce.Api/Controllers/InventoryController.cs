using Ecommerce.Api.Models;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
           _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetAll()
        {
            var items =  await _inventoryService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<Inventory>> GetByProductId(int productId)
        {
            var item = await _inventoryService.GetByProductIdAsync(productId);
            if (item == null) 
            {
                return NotFound(item);
            }
            return Ok(item);
           
        }
        [HttpPost]
        public async Task<ActionResult<Inventory>> Add(Inventory item)
        {
            var created = await _inventoryService.AddAsync(item);

            return CreatedAtAction(nameof(GetByProductId), new { productId = created.ProductId }, created);
        }

        [HttpPut("product/{productId}")]
        public async Task<IActionResult> UpdateQuantity(int productId, [FromBody] int newQuantity)
        {
            var success = await _inventoryService.UpdateQuantityAsync(productId, newQuantity);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{inventoryId}")]
        public async Task<IActionResult> Delete(int inventoryId)
        {
            var success = await _inventoryService.DeleteAsync(inventoryId);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
