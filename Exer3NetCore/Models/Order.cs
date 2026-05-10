namespace Exer3NetCore.Models;

public class Order
{
    public string OrderId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string AgentId { get; set; } = string.Empty;
}
