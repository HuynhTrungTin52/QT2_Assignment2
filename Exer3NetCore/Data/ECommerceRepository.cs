using System.Data;
using Microsoft.Data.SqlClient;
using Exer3NetCore.Models;

namespace Exer3NetCore.Data;

public class ECommerceRepository
{
    private readonly string _connectionString;

    public ECommerceRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ECommerce") ?? string.Empty;
    }

    public async Task<UserAccount?> GetUserAsync(string userName, string password)
    {
        const string sql = "SELECT UserID, UserName, Email, Password, IsLocked FROM Users WHERE UserName = @UserName AND Password = @Password";

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserName", userName);
        command.Parameters.AddWithValue("@Password", password);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new UserAccount
        {
            UserId = reader.GetInt32(0),
            UserName = reader.GetString(1),
            Email = reader.GetString(2),
            Password = reader.GetString(3),
            IsLocked = reader.GetBoolean(4)
        };
    }

    public async Task<List<Agent>> GetAgentsAsync()
    {
        const string sql = "SELECT AgentID, AgentName, Address FROM Agent ORDER BY AgentName";
        var agents = new List<Agent>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            agents.Add(new Agent
            {
                AgentId = reader.GetString(0),
                AgentName = reader.GetString(1),
                Address = reader.GetString(2)
            });
        }

        return agents;
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        const string sql = "SELECT ItemID, ItemName, Size, Price FROM Item ORDER BY ItemName";
        var items = new List<Item>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            items.Add(new Item
            {
                ItemId = reader.GetString(0),
                ItemName = reader.GetString(1),
                Size = reader.GetString(2),
                Price = reader.GetDecimal(3)
            });
        }

        return items;
    }

    public async Task AddAgentAsync(Agent agent)
    {
        const string sql = "INSERT INTO Agent (AgentID, AgentName, Address) VALUES (@AgentID, @AgentName, @Address)";

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@AgentID", agent.AgentId);
        command.Parameters.AddWithValue("@AgentName", agent.AgentName);
        command.Parameters.AddWithValue("@Address", agent.Address);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task AddItemAsync(Item item)
    {
        const string sql = "INSERT INTO Item (ItemID, ItemName, Size, Price) VALUES (@ItemID, @ItemName, @Size, @Price)";

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ItemID", item.ItemId);
        command.Parameters.AddWithValue("@ItemName", item.ItemName);
        command.Parameters.AddWithValue("@Size", item.Size);
        command.Parameters.AddWithValue("@Price", item.Price);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task AddOrderAsync(Order order, IEnumerable<OrderDetail> details)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            const string orderSql = "INSERT INTO Orders (OrderID, OrderDate, AgentID) VALUES (@OrderID, @OrderDate, @AgentID)";
            await using (var orderCommand = new SqlCommand(orderSql, connection, transaction))
            {
                orderCommand.Parameters.AddWithValue("@OrderID", order.OrderId);
                orderCommand.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                orderCommand.Parameters.AddWithValue("@AgentID", order.AgentId);
                await orderCommand.ExecuteNonQueryAsync();
            }

            const string detailSql = "INSERT INTO OrderDetail (OrderID, ItemID, Quantity, UnitAmount) VALUES (@OrderID, @ItemID, @Quantity, @UnitAmount)";
            foreach (var detail in details)
            {
                await using var detailCommand = new SqlCommand(detailSql, connection, transaction);
                detailCommand.Parameters.AddWithValue("@OrderID", detail.OrderId);
                detailCommand.Parameters.AddWithValue("@ItemID", detail.ItemId);
                detailCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);
                detailCommand.Parameters.AddWithValue("@UnitAmount", detail.UnitAmount);
                await detailCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<(Order? Order, List<OrderDetail> Details)> GetOrderAsync(string orderId)
    {
        const string orderSql = "SELECT OrderID, OrderDate, AgentID FROM Orders WHERE OrderID = @OrderID";
        const string detailSql = "SELECT ID, OrderID, ItemID, Quantity, UnitAmount FROM OrderDetail WHERE OrderID = @OrderID";

        await using var connection = new SqlConnection(_connectionString);
        await using var orderCommand = new SqlCommand(orderSql, connection);
        orderCommand.Parameters.AddWithValue("@OrderID", orderId);

        await connection.OpenAsync();
        await using var reader = await orderCommand.ExecuteReaderAsync(CommandBehavior.SingleRow);

        if (!await reader.ReadAsync())
        {
            return (null, new List<OrderDetail>());
        }

        var order = new Order
        {
            OrderId = reader.GetString(0),
            OrderDate = reader.GetDateTime(1),
            AgentId = reader.GetString(2)
        };

        await reader.CloseAsync();

        var details = new List<OrderDetail>();
        await using var detailCommand = new SqlCommand(detailSql, connection);
        detailCommand.Parameters.AddWithValue("@OrderID", orderId);
        await using var detailReader = await detailCommand.ExecuteReaderAsync();

        while (await detailReader.ReadAsync())
        {
            details.Add(new OrderDetail
            {
                Id = detailReader.GetInt32(0),
                OrderId = detailReader.GetString(1),
                ItemId = detailReader.GetString(2),
                Quantity = detailReader.GetInt32(3),
                UnitAmount = detailReader.GetDecimal(4)
            });
        }

        return (order, details);
    }

    public async Task<List<BestItemResult>> GetBestItemsAsync(DateTime? startDate, DateTime? endDate, int? minQuantity, string? size, string? agentId)
    {
        var sql = @"
SELECT i.ItemID, i.ItemName, i.Size, SUM(od.Quantity * od.UnitAmount) AS Revenue
FROM OrderDetail od
INNER JOIN Orders o ON o.OrderID = od.OrderID
INNER JOIN Item i ON i.ItemID = od.ItemID
WHERE (@StartDate IS NULL OR o.OrderDate >= @StartDate)
  AND (@EndDate IS NULL OR o.OrderDate <= @EndDate)
  AND (@MinQuantity IS NULL OR od.Quantity >= @MinQuantity)
  AND (@Size IS NULL OR i.Size = @Size)
  AND (@AgentId IS NULL OR o.AgentID = @AgentId)
GROUP BY i.ItemID, i.ItemName, i.Size
ORDER BY Revenue DESC;";

        var results = new List<BestItemResult>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StartDate", (object?)startDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@EndDate", (object?)endDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@MinQuantity", (object?)minQuantity ?? DBNull.Value);
        command.Parameters.AddWithValue("@Size", (object?)size ?? DBNull.Value);
        command.Parameters.AddWithValue("@AgentId", (object?)agentId ?? DBNull.Value);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new BestItemResult
            {
                ItemId = reader.GetString(0),
                ItemName = reader.GetString(1),
                Size = reader.GetString(2),
                Revenue = reader.GetDecimal(3)
            });
        }

        return results;
    }

    public async Task<List<OrderSummary>> GetOrderSummariesAsync()
    {
        const string sql = @"
SELECT o.OrderID, o.OrderDate, o.AgentID, SUM(od.Quantity * od.UnitAmount) AS TotalAmount
FROM Orders o
LEFT JOIN OrderDetail od ON o.OrderID = od.OrderID
GROUP BY o.OrderID, o.OrderDate, o.AgentID
ORDER BY o.OrderDate DESC;";

        var results = new List<OrderSummary>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new OrderSummary
            {
                OrderId = reader.GetString(0),
                OrderDate = reader.GetDateTime(1),
                AgentId = reader.GetString(2),
                TotalAmount = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3)
            });
        }

        return results;
    }

    public async Task<List<CustomerItemResult>> GetItemsPurchasedByCustomerAsync(string userName, DateTime? startDate, DateTime? endDate)
    {
        const string sql = @"
SELECT o.AgentID AS Customer, i.ItemName, i.Size, SUM(od.Quantity) AS Quantity, SUM(od.Quantity * od.UnitAmount) AS Total
FROM Orders o
INNER JOIN OrderDetail od ON o.OrderID = od.OrderID
INNER JOIN Item i ON i.ItemID = od.ItemID
WHERE o.AgentID = @UserName
  AND (@StartDate IS NULL OR o.OrderDate >= @StartDate)
  AND (@EndDate IS NULL OR o.OrderDate <= @EndDate)
GROUP BY o.AgentID, i.ItemName, i.Size
ORDER BY Total DESC;";

        var results = new List<CustomerItemResult>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserName", userName);
        command.Parameters.AddWithValue("@StartDate", (object?)startDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@EndDate", (object?)endDate ?? DBNull.Value);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new CustomerItemResult
            {
                Customer = reader.GetString(0),
                ItemName = reader.GetString(1),
                Size = reader.GetString(2),
                Quantity = reader.GetInt32(3),
                Total = reader.GetDecimal(4)
            });
        }

        return results;
    }

    public async Task<List<CustomerItemResult>> GetCustomerPurchasesAsync(DateTime? startDate, DateTime? endDate, string? size, string? agentId)
    {
        const string sql = @"
SELECT o.AgentID AS Customer, i.ItemName, i.Size, SUM(od.Quantity) AS Quantity, SUM(od.Quantity * od.UnitAmount) AS Total
FROM Orders o
INNER JOIN OrderDetail od ON o.OrderID = od.OrderID
INNER JOIN Item i ON i.ItemID = od.ItemID
WHERE (@StartDate IS NULL OR o.OrderDate >= @StartDate)
  AND (@EndDate IS NULL OR o.OrderDate <= @EndDate)
  AND (@Size IS NULL OR i.Size = @Size)
  AND (@AgentId IS NULL OR o.AgentID = @AgentId)
GROUP BY o.AgentID, i.ItemName, i.Size
ORDER BY Total DESC;";

        var results = new List<CustomerItemResult>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StartDate", (object?)startDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@EndDate", (object?)endDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@Size", (object?)size ?? DBNull.Value);
        command.Parameters.AddWithValue("@AgentId", (object?)agentId ?? DBNull.Value);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new CustomerItemResult
            {
                Customer = reader.GetString(0),
                ItemName = reader.GetString(1),
                Size = reader.GetString(2),
                Quantity = reader.GetInt32(3),
                Total = reader.GetDecimal(4)
            });
        }

        return results;
    }
}

public class BestItemResult
{
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

public class CustomerItemResult
{
    public string Customer { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Total { get; set; }
}
