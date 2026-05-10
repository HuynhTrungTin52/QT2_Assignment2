namespace Exer3NetCore.Models;

public class OrderDetail
{
    public int Id { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string ItemId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitAmount { get; set; }
}
