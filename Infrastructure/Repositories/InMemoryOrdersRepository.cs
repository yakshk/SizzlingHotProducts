using System.Text.Json;
using Application.Interfaces;
using Application.Models;
using Infrastructure.Common;

namespace Infrastructure.Repositories;

public class InMemoryOrdersRepository : IOrdersRepository
{
    private readonly List<Order> _orders = [];

    public InMemoryOrdersRepository()
    {
        LoadSeedData();
    }

    public Task<List<Order>> GetAllAsync()
    {
        return Task.FromResult(_orders.ToList());
    }

    private void LoadSeedData()
    {
        var filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Seeding",
            "orders.json"
        );

        if (!File.Exists(filePath)) throw new FileNotFoundException($"Seed file not found at: {filePath}");

        var json = File.ReadAllText(filePath);

        var orders = JsonSerializer.Deserialize<List<Order>>(json, JsonDefaults.Options);

        if (orders is not null) _orders.AddRange(orders);
    }
}