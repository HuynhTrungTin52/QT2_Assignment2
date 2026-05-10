using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exer4MVCNetCore.Data;
using Exer4MVCNetCore.Models;

namespace Exer4MVCNetCore.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly ECommerceService _service;

    public ReportsController(ECommerceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> BestItems()
    {
        var items = await _service.GetBestItemsAsync();
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BestItems(DateTime? startDate, DateTime? endDate, int? minQuantity, string? size, string? agentId)
    {
        var items = await _service.GetBestItemsAsync(startDate, endDate, minQuantity, size, agentId);
        ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
        ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
        ViewBag.MinQuantity = minQuantity;
        ViewBag.Size = size;
        ViewBag.AgentId = agentId;
        ViewBag.Agents = await _service.GetAllAgentsAsync();
        ViewBag.Items = await _service.GetAllItemsAsync();
        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> CustomerPurchases()
    {
        var agents = await _service.GetAllAgentsAsync();
        ViewBag.Agents = agents;
        return View(new List<CustomerItemResult>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CustomerPurchases(string agentId, DateTime? startDate, DateTime? endDate)
    {
        if (string.IsNullOrEmpty(agentId))
        {
            ModelState.AddModelError("agentId", "Please select an agent.");
            var agents = await _service.GetAllAgentsAsync();
            ViewBag.Agents = agents;
            return View(new List<CustomerItemResult>());
        }

        var results = await _service.GetItemsPurchasedByAgentAsync(agentId, startDate, endDate);
        ViewBag.SelectedAgentId = agentId;
        ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
        ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
        ViewBag.Agents = await _service.GetAllAgentsAsync();
        return View(results);
    }
}
