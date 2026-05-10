namespace Exer4MVCNetCore.Models;

public class Agent
{
    public string AgentId { get; set; } = string.Empty;
    public string AgentName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
