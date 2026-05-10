using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exer4MVCNetCore.Data;
using Exer4MVCNetCore.Models;

namespace Exer4MVCNetCore.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly ECommerceService _service;

    public OrdersController(ECommerceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var orders = await _service.GetAllOrdersAsync();
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Agents = await _service.GetAllAgentsAsync();
        ViewBag.Items = await _service.GetAllItemsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Order order, string itemIds, string quantities, string unitAmounts)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Agents = await _service.GetAllAgentsAsync();
            ViewBag.Items = await _service.GetAllItemsAsync();
            return View(order);
        }

        try
        {
            // Generate OrderId
            order.OrderId = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss");
            order.OrderDate = DateTime.Now;

            var itemIdList = itemIds?.Split(',') ?? Array.Empty<string>();
            var quantityList = (quantities?.Split(',').Select(int.Parse) ?? Array.Empty<int>()).ToArray();
            var unitAmountList = (unitAmounts?.Split(',').Select(decimal.Parse) ?? Array.Empty<decimal>()).ToArray();

            var details = new List<OrderDetail>();
            for (int i = 0; i < itemIdList.Length; i++)
            {
                if (!string.IsNullOrEmpty(itemIdList[i]))
                {
                    details.Add(new OrderDetail
                    {
                        ItemId = itemIdList[i],
                        Quantity = quantityList[i],
                        UnitAmount = unitAmountList[i]
                    });
                }
            }

            if (details.Count == 0)
            {
                ModelState.AddModelError("", "At least one item is required.");
                ViewBag.Agents = await _service.GetAllAgentsAsync();
                ViewBag.Items = await _service.GetAllItemsAsync();
                return View(order);
            }

            await _service.AddOrderAsync(order, details);
            TempData["SuccessMessage"] = "Order created successfully!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error: {ex.Message}");
            ViewBag.Agents = await _service.GetAllAgentsAsync();
            ViewBag.Items = await _service.GetAllItemsAsync();
            return View(order);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var (order, details) = await _service.GetOrderWithDetailsAsync(id);
        if (order == null)
            return NotFound();

        ViewBag.Details = details;
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Print(string id)
    {
        var (order, details) = await _service.GetOrderWithDetailsAsync(id);
        if (order == null)
            return NotFound();

        ViewBag.Details = details;
        ViewBag.IsPrint = true;
        return View("Details", order);
    }
}
