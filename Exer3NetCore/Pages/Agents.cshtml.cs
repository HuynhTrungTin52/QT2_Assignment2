using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Exer3NetCore.Data;
using Exer3NetCore.Models;

namespace Exer3NetCore.Pages;

[Authorize]
public class AgentsModel : PageModel
{
    private readonly ECommerceRepository _repository;

    public AgentsModel(ECommerceRepository repository)
    {
        _repository = repository;
    }

    public List<Agent> Agents { get; private set; } = new();

    [BindProperty]
    public AgentInput NewAgent { get; set; } = new();

    public async Task OnGetAsync()
    {
        Agents = await _repository.GetAgentsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Agents = await _repository.GetAgentsAsync();
            return Page();
        }

        await _repository.AddAgentAsync(new Agent
        {
            AgentId = NewAgent.AgentId,
            AgentName = NewAgent.AgentName,
            Address = NewAgent.Address
        });

        return RedirectToPage();
    }

    public class AgentInput
    {
        [Required]
        public string AgentId { get; set; } = string.Empty;

        [Required]
        public string AgentName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;
    }
}
