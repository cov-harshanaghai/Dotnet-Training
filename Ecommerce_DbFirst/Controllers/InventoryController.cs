using Ecommerce_DBFirst.Models;
using Ecommerce_DBFirst.Services;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce_DBFirst.Controllers;

public class InventoryController : Controller
{
    private readonly IInventoryApiService _inventoryApi;
    private readonly ILogger<InventoryController> _logger;

    public InventoryController(IInventoryApiService inventoryApi, ILogger<InventoryController> logger)
    {
        _inventoryApi = inventoryApi;
        _logger = logger;
    }

    // GET: /Inventory
    public async Task<IActionResult> Index()
    {
        var items = await _inventoryApi.GetAllAsync();
        return View(items);
    }

    // GET: /Inventory/Details/2006
    public async Task<IActionResult> Details(int productId)
    {
        var item = await _inventoryApi.GetByProductIdAsync(productId);

        if (item == null)
        {
            TempData["ErrorMessage"] = $"No inventory record found for product {productId}.";
            return RedirectToAction(nameof(Index));
        }

        return View(item);
    }

    // GET: /Inventory/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Inventory/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Inventory item)
    {
        if (!ModelState.IsValid)
        {
            return View(item);
        }

        var created = await _inventoryApi.AddAsync(item);

        if (created == null)
        {
            ModelState.AddModelError("", "Could not create inventory record. The Inventory service may be unavailable.");
            return View(item);
        }

        TempData["SuccessMessage"] = "Inventory record created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Inventory/Edit/2006
    public async Task<IActionResult> Edit(int productId)
    {
        var item = await _inventoryApi.GetByProductIdAsync(productId);

        if (item == null)
        {
            TempData["ErrorMessage"] = $"No inventory record found for product {productId}.";
            return RedirectToAction(nameof(Index));
        }

        return View(item);
    }

    // POST: /Inventory/Edit/2006
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int productId, int quantityInStock)
    {
        var success = await _inventoryApi.UpdateQuantityAsync(productId, quantityInStock);

        if (!success)
        {
            TempData["ErrorMessage"] = "Could not update stock quantity. Please try again later.";
            return RedirectToAction(nameof(Edit), new { productId });
        }

        TempData["SuccessMessage"] = "Stock quantity updated.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Inventory/Delete/3
    public async Task<IActionResult> Delete(int inventoryId, int productId)
    {
        var item = await _inventoryApi.GetByProductIdAsync(productId);

        if (item == null)
        {
            TempData["ErrorMessage"] = "Inventory record not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(item);
    }

    // POST: /Inventory/Delete/3
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int inventoryId)
    {
        var success = await _inventoryApi.DeleteAsync(inventoryId);

        TempData[success ? "SuccessMessage" : "ErrorMessage"] =
            success ? "Inventory record deleted." : "Could not delete record. It may already be removed.";

        return RedirectToAction(nameof(Index));
    }
}