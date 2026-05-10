namespace Exer4MVCNetCore.Models;

public class CustomerItemResult
{
    public string Customer { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Total { get; set; }
}
