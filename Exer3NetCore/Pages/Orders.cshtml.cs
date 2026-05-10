using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Exer3NetCore.Data;
using Exer3NetCore.Models;

namespace Exer3NetCore.Pages;

[Authorize]
public class OrdersModel : PageModel
{
    private readonly ECommerceRepository _repository;

    public OrdersModel(ECommerceRepository repository)
    {
        _repository = repository;
    }

    [BindProperty]
    public OrderInput OrderForm { get; set; } = new();

    [BindProperty]
    public List<OrderDetailInput> DetailInputs { get; set; } = new();

    public List<SelectListItem> AgentOptions { get; private set; } = new();
    public List<SelectListItem> ItemOptions { get; private set; } = new();

    public string? StatusMessage { get; private set; }
    public string? CreatedOrderId { get; private set; }

    public async Task OnGetAsync()
    {
        await LoadOptionsAsync();
        EnsureDetailRows();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadOptionsAsync();
        EnsureDetailRows();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var order = new Order
        {
            OrderId = OrderForm.OrderId,
            OrderDate = OrderForm.OrderDate,
            AgentId = OrderForm.AgentId
        };

        var details = DetailInputs
            .Where(d => !string.IsNullOrWhiteSpace(d.ItemId))
            .Select(d => new OrderDetail
            {
                OrderId = order.OrderId,
                ItemId = d.ItemId,
                Quantity = d.Quantity,
                UnitAmount = d.UnitAmount
            })
            .ToList();

        if (details.Count == 0)
        {
            StatusMessage = "Please add at least one order detail.";
            return Page();
        }

        await _repository.AddOrderAsync(order, details);
        CreatedOrderId = order.OrderId;
        StatusMessage = "Order saved successfully.";
        return Page();
    }

    private async Task LoadOptionsAsync()
    {
        var agents = await _repository.GetAgentsAsync();
        var items = await _repository.GetItemsAsync();

        AgentOptions = agents.Select(a => new SelectListItem(a.AgentName, a.AgentId)).ToList();
        ItemOptions = items.Select(i => new SelectListItem($"{i.ItemName} ({i.Size})", i.ItemId)).ToList();
    }

    private void EnsureDetailRows()
    {
        var targetCount = Math.Max(1, OrderForm.DetailCount);

        if (DetailInputs.Count < targetCount)
        {
            while (DetailInputs.Count < targetCount)
            {
                DetailInputs.Add(new OrderDetailInput());
            }
        }
        else if (DetailInputs.Count > targetCount)
        {
            DetailInputs = DetailInputs.Take(targetCount).ToList();
        }
    }

    public class OrderInput
    {
        [Required]
        public string OrderId { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Today;

        [Required]
        public string AgentId { get; set; } = string.Empty;

        [Range(1, 20)]
        public int DetailCount { get; set; } = 1;
    }

    public class OrderDetailInput
    {
        [Required]
        public string ItemId { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Range(0.01, double.MaxValue)]
        public decimal UnitAmount { get; set; } = 1;
    }
}
