using Microsoft.EntityFrameworkCore;
using Exer4MVCNetCore.Models;

namespace Exer4MVCNetCore.Data;

public class ECommerceService
{
    private readonly ECommerceDbContext _context;

    public ECommerceService(ECommerceDbContext context)
    {
        _context = context;
    }

    public async Task<UserAccount?> AuthenticateAsync(string userName, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password && !u.IsLocked);
    }

    // Agent operations
    public async Task<List<Agent>> GetAllAgentsAsync()
    {
        return await _context.Agents.OrderBy(a => a.AgentName).ToListAsync();
    }

    public async Task<Agent?> GetAgentByIdAsync(string agentId)
    {
        return await _context.Agents.FirstOrDefaultAsync(a => a.AgentId == agentId);
    }

    public async Task AddAgentAsync(Agent agent)
    {
        _context.Agents.Add(agent);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAgentAsync(Agent agent)
    {
        // Check if agent exists first
        var existingAgent = await _context.Agents.FirstOrDefaultAsync(a => a.AgentId == agent.AgentId);
        if (existingAgent == null)
        {
            throw new InvalidOperationException($"Agent with ID {agent.AgentId} not found.");
        }

        // Update the existing tracked entity with new values
        existingAgent.AgentName = agent.AgentName;
        existingAgent.Address = agent.Address;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAgentAsync(string agentId)
    {
        var agent = await _context.Agents.FirstOrDefaultAsync(a => a.AgentId == agentId);
        if (agent != null)
        {
            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
        }
    }

    // Item operations
    public async Task<List<Item>> GetAllItemsAsync()
    {
        return await _context.Items.OrderBy(i => i.ItemName).ToListAsync();
    }

    public async Task<Item?> GetItemByIdAsync(string itemId)
    {
        return await _context.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);
    }

    public async Task AddItemAsync(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateItemAsync(Item item)
    {
        // Check if item exists first
        var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == item.ItemId);
        if (existingItem == null)
        {
            throw new InvalidOperationException($"Item with ID {item.ItemId} not found.");
        }

        // Update the existing tracked entity with new values
        existingItem.ItemName = item.ItemName;
        existingItem.Size = item.Size;
        existingItem.Price = item.Price;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteItemAsync(string itemId)
    {
        var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);
        if (item != null)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    // Order operations
    public async Task<List<OrderSummary>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderDetails)
            .Select(o => new OrderSummary
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                AgentId = o.AgentId,
                TotalAmount = o.OrderDetails.Sum(od => od.Quantity * od.UnitAmount)
            })
            .OrderByDescending(os => os.OrderDate)
            .ToListAsync();
    }

    public async Task<(Order? Order, List<OrderDetail> Details)> GetOrderWithDetailsAsync(string orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Item)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
            return (null, new List<OrderDetail>());

        return (order, order.OrderDetails.ToList());
    }

    public async Task AddOrderAsync(Order order, List<OrderDetail> details)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var detail in details)
            {
                detail.OrderId = order.OrderId;
                _context.OrderDetails.Add(detail);
            }
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<BestItemResult>> GetBestItemsAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        int? minQuantity = null,
        string? size = null,
        string? agentId = null)
    {
        var query = _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Item)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(od => od.Order!.OrderDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(od => od.Order!.OrderDate <= endDate.Value);

        if (minQuantity.HasValue)
            query = query.Where(od => od.Quantity >= minQuantity.Value);

        if (!string.IsNullOrEmpty(size))
            query = query.Where(od => od.Item!.Size == size);

        if (!string.IsNullOrEmpty(agentId))
            query = query.Where(od => od.Order!.AgentId == agentId);

        return await query
            .GroupBy(od => new { od.ItemId, od.Item!.ItemName, od.Item.Size })
            .Select(g => new BestItemResult
            {
                ItemId = g.Key.ItemId,
                ItemName = g.Key.ItemName,
                Size = g.Key.Size,
                Revenue = g.Sum(od => od.Quantity * od.UnitAmount)
            })
            .OrderByDescending(r => r.Revenue)
            .ToListAsync();
    }

    public async Task<List<CustomerItemResult>> GetItemsPurchasedByAgentAsync(
        string agentId,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var query = _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Item)
            .Where(od => od.Order!.AgentId == agentId);

        if (startDate.HasValue)
            query = query.Where(od => od.Order!.OrderDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(od => od.Order!.OrderDate <= endDate.Value);

        return await query
            .GroupBy(od => new { od.Item!.ItemName, od.Item.Size })
            .Select(g => new CustomerItemResult
            {
                Customer = agentId,
                ItemName = g.Key.ItemName,
                Size = g.Key.Size,
                Quantity = g.Sum(od => od.Quantity),
                Total = g.Sum(od => od.Quantity * od.UnitAmount)
            })
            .OrderByDescending(r => r.Total)
            .ToListAsync();
    }
}
