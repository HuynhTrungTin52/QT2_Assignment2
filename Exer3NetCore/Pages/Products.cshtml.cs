using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Exer3NetCore.Data;
using Exer3NetCore.Models;

namespace Exer3NetCore.Pages;

[Authorize]
public class ProductsModel : PageModel
{
    private readonly ECommerceRepository _repository;

    public ProductsModel(ECommerceRepository repository)
    {
        _repository = repository;
    }

    public List<Item> Items { get; private set; } = new();

    [BindProperty]
    public ItemInput NewItem { get; set; } = new();

    public async Task OnGetAsync()
    {
        Items = await _repository.GetItemsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Items = await _repository.GetItemsAsync();
            return Page();
        }

        await _repository.AddItemAsync(new Item
        {
            ItemId = NewItem.ItemId,
            ItemName = NewItem.ItemName,
            Size = NewItem.Size,
            Price = NewItem.Price
        });

        return RedirectToPage();
    }

    public class ItemInput
    {
        [Required]
        public string ItemId { get; set; } = string.Empty;

        [Required]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public string Size { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
