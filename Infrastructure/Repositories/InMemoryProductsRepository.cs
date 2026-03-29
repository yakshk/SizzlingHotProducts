using System.Text.Json;
using Application.Interfaces;
using Application.Models;
using Infrastructure.Common;

namespace Infrastructure.Repositories;

public class InMemoryProductsRepository : IProductsRepository
{
    private readonly List<Product> _products = [];

    public InMemoryProductsRepository()
    {
        LoadSeedData();
    }

    public Task<List<Product>> GetAllAsync()
    {
        return Task.FromResult(_products.ToList());
    }

    private void LoadSeedData()
    {
        var filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Seeding",
            "products.json"
        );

        if (!File.Exists(filePath)) throw new FileNotFoundException($"Seed file not found at: {filePath}");

        var json = File.ReadAllText(filePath);

        var products = JsonSerializer.Deserialize<List<Product>>(json, JsonDefaults.Options);

        if (products is not null) _products.AddRange(products);
    }
}