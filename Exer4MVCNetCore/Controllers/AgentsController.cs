using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exer4MVCNetCore.Data;
using Exer4MVCNetCore.Models;

namespace Exer4MVCNetCore.Controllers;

[Authorize]
public class AgentsController : Controller
{
    private readonly ECommerceService _service;

    public AgentsController(ECommerceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var agents = await _service.GetAllAgentsAsync();
        return View(agents);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Agent agent)
    {
        if (!ModelState.IsValid)
            return View(agent);

        try
        {
            await _service.AddAgentAsync(agent);
            TempData["SuccessMessage"] = "Agent added successfully!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error: {ex.Message}");
            return View(agent);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var agent = await _service.GetAgentByIdAsync(id);
        if (agent == null)
            return NotFound();

        return View(agent);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Agent agent)
    {
        if (!ModelState.IsValid)
            return View(agent);

        try
        {
            await _service.UpdateAgentAsync(agent);
            TempData["SuccessMessage"] = "Agent updated successfully!";
            return RedirectToAction("Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", "Agent not found. It may have been deleted.");
            return View(agent);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error: {ex.Message}");
            return View(agent);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _service.DeleteAgentAsync(id);
            TempData["SuccessMessage"] = "Agent deleted successfully!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
}
