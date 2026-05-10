using Exer4MVCNetCore.Models;

namespace Exer4MVCNetCore.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ECommerceDbContext context)
    {
        try
        {
            // Seed Users
            if (!context.Users.Any())
            {
                var users = new List<UserAccount>
                {
                    new UserAccount { UserName = "baonhi", Email = "nhi@tdtu.edu.vn", Password = "123", IsLocked = false },
                    new UserAccount { UserName = "thaonhu", Email = "nhu@tdtu.edu.vn", Password = "123", IsLocked = false },
                    new UserAccount { UserName = "minhnghia", Email = "nghia@tdtu.edu.vn", Password = "123", IsLocked = false },
                    new UserAccount { UserName = "trungtin", Email = "tin@tdtu.edu.vn", Password = "123", IsLocked = false },
                    new UserAccount { UserName = "admin", Email = "admin@tdtu.edu.vn", Password = "123", IsLocked = false }
                };
                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }

            // Seed Agents
            if (!context.Agents.Any())
            {
                var agents = new List<Agent>
                {
                    new Agent { AgentId = "AG01", AgentName = "Đại lý Bảo Nhi", Address = "Quận 7, TP.HCM" },
                    new Agent { AgentId = "AG02", AgentName = "Đại lý Thảo Như", Address = "Quận 7, TP.HCM" },
                    new Agent { AgentId = "AG03", AgentName = "Đại lý Minh Nghĩa", Address = "Quận 1, TP.HCM" },
                    new Agent { AgentId = "AG04", AgentName = "Đại lý Trung Tín", Address = "Bình Chánh, TP.HCM" },
                    new Agent { AgentId = "AG05", AgentName = "Đại lý Đà Nẵng", Address = "Hải Châu, Đà Nẵng" },
                    new Agent { AgentId = "AG06", AgentName = "Đại lý Hà Nội", Address = "Hoàn Kiếm, Hà Nội" },
                    new Agent { AgentId = "AG07", AgentName = "Đại lý Bình Dương", Address = "Thủ Dầu Một" },
                    new Agent { AgentId = "AG08", AgentName = "Đại lý Đồng Nai", Address = "Biên Hòa" },
                    new Agent { AgentId = "AG09", AgentName = "Đại lý Long An", Address = "Tân An" },
                    new Agent { AgentId = "AG10", AgentName = "Đại lý Vũng Tàu", Address = "TP. Vũng Tàu" },
                    new Agent { AgentId = "AG11", AgentName = "Đại lý Nha Trang", Address = "Khánh Hòa" },
                    new Agent { AgentId = "AG12", AgentName = "Đại lý Đà Lạt", Address = "Lâm Đồng" },
                    new Agent { AgentId = "AG13", AgentName = "Đại lý Huế", Address = "Thừa Thiên Huế" },
                    new Agent { AgentId = "AG14", AgentName = "Đại lý Hải Phòng", Address = "Lê Chân" },
                    new Agent { AgentId = "AG15", AgentName = "Đại lý Quảng Ngãi", Address = "TP. Quảng Ngãi" }
                };
                context.Agents.AddRange(agents);
                await context.SaveChangesAsync();
            }

            // Seed Items
            if (!context.Items.Any())
            {
                var items = new List<Item>
                {
                    new Item { ItemId = "IT01", ItemName = "Laptop Lenovo IdeaPad", Size = "14 inch", Price = 15000000 },
                    new Item { ItemId = "IT02", ItemName = "Chuột Logitech G304", Size = "S", Price = 800000 },
                    new Item { ItemId = "IT03", ItemName = "Bàn phím cơ Akko", Size = "Fullsize", Price = 1200000 },
                    new Item { ItemId = "IT04", ItemName = "Màn hình Dell Ultrasharp", Size = "24 inch", Price = 5500000 },
                    new Item { ItemId = "IT05", ItemName = "Tai nghe Sony WH-1000XM4", Size = "L", Price = 6500000 },
                    new Item { ItemId = "IT06", ItemName = "Loa Bluetooth Marshall", Size = "M", Price = 4000000 },
                    new Item { ItemId = "IT07", ItemName = "Ổ cứng SSD Samsung", Size = "500GB", Price = 1500000 },
                    new Item { ItemId = "IT08", ItemName = "Ram Kingston HyperX", Size = "16GB", Price = 1800000 },
                    new Item { ItemId = "IT09", ItemName = "Card đồ họa RTX 3060", Size = "Triple Fan", Price = 9000000 },
                    new Item { ItemId = "IT10", ItemName = "Nguồn Corsair 750W", Size = "Standard", Price = 2000000 },
                    new Item { ItemId = "IT11", ItemName = "Vỏ case Case Master", Size = "ATX", Price = 1300000 },
                    new Item { ItemId = "IT12", ItemName = "Tản nhiệt khí Deepcool", Size = "Tower", Price = 700000 },
                    new Item { ItemId = "IT13", ItemName = "Webcam Logitech C922", Size = "Mini", Price = 2200000 },
                    new Item { ItemId = "IT14", ItemName = "Bàn di chuột SteelSeries", Size = "Large", Price = 500000 },
                    new Item { ItemId = "IT15", ItemName = "Cáp HDMI 2.1", Size = "2 meter", Price = 300000 }
                };
                context.Items.AddRange(items);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            throw;
        }
    }
}
