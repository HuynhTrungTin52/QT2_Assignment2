using Microsoft.EntityFrameworkCore;
using Exer4MVCNetCore.Models;

namespace Exer4MVCNetCore.Data;

public class ECommerceDbContext : DbContext
{
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
    {
    }

    public DbSet<UserAccount> Users { get; set; } = null!;
    public DbSet<Agent> Agents { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<UserAccount>()
            .HasKey(u => u.UserId);
        modelBuilder.Entity<UserAccount>()
            .Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<UserAccount>()
            .Property(u => u.Email)
            .HasMaxLength(100);
        modelBuilder.Entity<UserAccount>()
            .Property(u => u.Password)
            .HasMaxLength(50);

        // Agent configuration
        modelBuilder.Entity<Agent>()
            .HasKey(a => a.AgentId);
        modelBuilder.Entity<Agent>()
            .Property(a => a.AgentName)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<Agent>()
            .Property(a => a.Address)
            .HasMaxLength(200);

        // Item configuration
        modelBuilder.Entity<Item>()
            .HasKey(i => i.ItemId);
        modelBuilder.Entity<Item>()
            .Property(i => i.ItemName)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<Item>()
            .Property(i => i.Size)
            .HasMaxLength(50);
        modelBuilder.Entity<Item>()
            .Property(i => i.Price)
            .HasPrecision(18, 2);

        // Order configuration
        modelBuilder.Entity<Order>()
            .HasKey(o => o.OrderId);
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Agent)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.AgentId)
            .OnDelete(DeleteBehavior.Restrict);

        // OrderDetail configuration
        modelBuilder.Entity<OrderDetail>()
            .HasKey(od => od.Id);
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Item)
            .WithMany(i => i.OrderDetails)
            .HasForeignKey(od => od.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<OrderDetail>()
            .Property(od => od.UnitAmount)
            .HasPrecision(18, 2);
    }
}
