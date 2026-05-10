using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Exer3NetCore.Data;

namespace Exer3NetCore.Pages;

[Authorize]
public class ReportsModel : PageModel
{
    private readonly ECommerceRepository _repository;

    public ReportsModel(ECommerceRepository repository)
    {
        _repository = repository;
    }

    public List<SelectListItem> AgentOptions { get; private set; } = new();

    public List<BestItemResult> BestItems { get; private set; } = new();
    public List<CustomerItemResult> CustomerItems { get; private set; } = new();
    public List<CustomerItemResult> CustomerPurchases { get; private set; } = new();
    public List<Exer3NetCore.Models.OrderSummary> OrderSummaries { get; private set; } = new();

    [BindProperty(SupportsGet = true)]
    public BestFilterInput BestFilter { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public CustomerItemsFilterInput CustomerItemsFilter { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public CustomerPurchasesFilterInput CustomerPurchasesFilter { get; set; } = new();

    public async Task OnGetAsync()
    {
        var agents = await _repository.GetAgentsAsync();
        AgentOptions = agents.Select(a => new SelectListItem(a.AgentName, a.AgentId)).ToList();

        BestItems = await _repository.GetBestItemsAsync(
            BestFilter.StartDate,
            BestFilter.EndDate,
            BestFilter.MinQuantity,
            BestFilter.Size,
            BestFilter.AgentId);

        CustomerItems = await _repository.GetItemsPurchasedByCustomerAsync(
            CustomerItemsFilter.Customer,
            CustomerItemsFilter.StartDate,
            CustomerItemsFilter.EndDate);

        CustomerPurchases = await _repository.GetCustomerPurchasesAsync(
            CustomerPurchasesFilter.StartDate,
            CustomerPurchasesFilter.EndDate,
            CustomerPurchasesFilter.Size,
            CustomerPurchasesFilter.AgentId);

        OrderSummaries = await _repository.GetOrderSummariesAsync();
    }

    public class BestFilterInput
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MinQuantity { get; set; }
        public string? Size { get; set; }
        public string? AgentId { get; set; }
    }

    public class CustomerItemsFilterInput
    {
        public string Customer { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class CustomerPurchasesFilterInput
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Size { get; set; }
        public string? AgentId { get; set; }
    }
}
