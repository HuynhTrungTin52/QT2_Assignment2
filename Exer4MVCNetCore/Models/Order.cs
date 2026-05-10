namespace Exer4MVCNetCore.Models;

public class Order
{
    public string OrderId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string AgentId { get; set; } = string.Empty;

    public Agent? Agent { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
