using Application.DTOs;
using Application.Enums;
using Application.Interfaces;
using Application.Requests;
using MediatR;

namespace Application.Handlers;

public class HotProductHandler(IProductsRepository productsRepo, IOrdersRepository ordersRepo)
    : IRequestHandler<HotProductRequest, HotProductDto?>
{
    public async Task<HotProductDto?> Handle(HotProductRequest request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate) return null;

        var allOrders = await ordersRepo.GetAllAsync();
        var allProducts = await productsRepo.GetAllAsync();

        var sales = new HashSet<(string CustomerId, string ProductId)>();

        foreach (var order in allOrders)
        {
            if (order.Date < request.StartDate || order.Date > request.EndDate) continue;
            if (order.Entries == null) continue;

            switch (order.Status)
            {
                case OrderEnum.OrderStatus.Completed:
                    foreach (var entry in order.Entries)
                        sales.Add((order.CustomerId, entry.Id));
                    break;
                case OrderEnum.OrderStatus.Cancelled:
                    foreach (var entry in order.Entries)
                        sales.Remove((order.CustomerId, entry.Id));
                    break;
                case OrderEnum.OrderStatus.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Count occurrences per product
        var productCounts = sales
            .GroupBy(s => s.ProductId)
            .ToDictionary(g => g.Key, g => g.Count());

        if (productCounts.Count == 0)
            return null;

        var productDict = allProducts.ToDictionary(p => p.Id);

        // Use the name as well to find the hottest product
        var hotProductKvp = productCounts
            .OrderByDescending(pc => pc.Value)
            .ThenBy(pc => productDict[pc.Key].Name)
            .First();

        return new HotProductDto
        {
            Id = hotProductKvp.Key,
            Name = productDict[hotProductKvp.Key].Name,
            Quantity = hotProductKvp.Value
        };
    }
}