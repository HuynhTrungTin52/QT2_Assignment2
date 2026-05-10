using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exer4MVCNetCore.Data;
using Exer4MVCNetCore.Models;

namespace Exer4MVCNetCore.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly ECommerceService _service;

    public ProductsController(ECommerceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var items = await _service.GetAllItemsAsync();
        return View(items);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Item item)
    {
        if (!ModelState.IsValid)
            return View(item);

        try
        {
            await _service.AddItemAsync(item);
            TempData["SuccessMessage"] = "Product added successfully!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error: {ex.Message}");
            return View(item);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var item = await _service.GetItemByIdAsync(id);
        if (item == null)
            return NotFound();

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Item item)
    {
        if (!ModelState.IsValid)
            return View(item);

        try
        {
            await _service.UpdateItemAsync(item);
            TempData["SuccessMessage"] = "Product updated successfully!";
            return RedirectToAction("Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", "Product not found. It may have been deleted.");
            return View(item);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error: {ex.Message}");
            return View(item);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _service.DeleteItemAsync(id);
            TempData["SuccessMessage"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
}
