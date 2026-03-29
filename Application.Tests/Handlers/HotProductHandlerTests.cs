using Application.Enums;
using Application.Handlers;
using Application.Interfaces;
using Application.Models;
using Application.Requests;
using Moq;
using Xunit;

namespace Application.Tests.Handlers;

public class HotProductHandlerTests
{
    private readonly Mock<IProductsRepository> _productsRepo = new();
    private readonly Mock<IOrdersRepository> _ordersRepo = new();

    private HotProductHandler CreateHandler() => new(_productsRepo.Object, _ordersRepo.Object);

    [Fact]
    public async Task Handle_ReturnsNull_WhenStartDateAfterEndDate()
    {
        var handler = CreateHandler();

        var request = new HotProductRequest
        {
            StartDate = new DateOnly(2024, 2, 1),
            EndDate = new DateOnly(2024, 1, 1)
        };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_IgnoresOrdersOutsideDateRange()
    {
        // Arrange
        var orders = new List<Order>
        {
            new()
            {
                CustomerId = "C1",
                Date = new DateOnly(2024, 1, 1),
                Status = OrderEnum.OrderStatus.Completed,
                Entries = [new EntryModel { Id = "P1", Quantity = 1 }],
                OrderId = "O10"
            }
        };

        _ordersRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);
        _productsRepo.Setup(r => r.GetAllAsync()).ReturnsAsync([new Product { Id = "P1", Name = "Product 1" }]);

        var handler = CreateHandler();

        var request = new HotProductRequest
        {
            StartDate = new DateOnly(2024, 1, 2),
            EndDate = new DateOnly(2024, 1, 3)
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_AddsSalesForCompletedOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order
            {
                OrderId = "O10",
                CustomerId = "C1",
                Date = new DateOnly(2024, 1, 2),
                Status = OrderEnum.OrderStatus.Completed,
                Entries = [new EntryModel { Id = "P1", Quantity = 1 }]
            }
        };

        _ordersRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);
        _productsRepo.Setup(r => r.GetAllAsync()).ReturnsAsync([new Product { Id = "P1", Name = "Coffee" }]);

        var handler = CreateHandler();

        var request = new HotProductRequest
        {
            StartDate = new DateOnly(2024, 1, 1),
            EndDate = new DateOnly(2024, 1, 5)
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("P1", result.Id);
        Assert.Equal("Coffee", result.Name);
        Assert.Equal(1, result.Quantity);
    }

    [Fact]
    public async Task Handle_RemovesSalesForCancelledOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order
            {
                OrderId = "O10",
                CustomerId = "C1",
                Date = new DateOnly(2024, 1, 2),
                Status = OrderEnum.OrderStatus.Completed,
                Entries = [new EntryModel { Id = "P1", Quantity = 1 }]
            },
            new Order
            {
                OrderId = "O20",
                CustomerId = "C1",
                Date = new DateOnly(2024, 1, 3),
                Status = OrderEnum.OrderStatus.Cancelled,
                Entries = [new EntryModel { Id = "P1", Quantity = 1 }]
            }
        };

        _ordersRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);
        _productsRepo.Setup(r => r.GetAllAsync()).ReturnsAsync([new Product { Id = "P1", Name = "Coffee" }]);

        var handler = CreateHandler();

        var request = new HotProductRequest
        {
            StartDate = new DateOnly(2024, 1, 1),
            EndDate = new DateOnly(2024, 1, 5)
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result); // sale added then removed
    }

    [Fact]
    public async Task Handle_ThrowsForUnknownStatus()
    {
        var orders = new List<Order>
        {
            new()
            {
                OrderId = "O10",
                CustomerId = "C1",
                Date = new DateOnly(2024, 1, 2),
                Status = OrderEnum.OrderStatus.Unknown,
                Entries = [new EntryModel { Id = "P1", Quantity = 1 }]
            }
        };

        _ordersRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);
        _productsRepo.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        var handler = CreateHandler();

        var request = new HotProductRequest
        {
            StartDate = new DateOnly(2024, 1, 1),
            EndDate = new DateOnly(2024, 1, 5)
        };

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UsesNameAsTieBreaker()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order
            {
                OrderId = "O10",
                CustomerId = "C1",
                Date = new DateOnly(2024, 1, 2),
                Status = OrderEnum.OrderStatus.Completed,
                Entries =
                [
                    new EntryModel { Id = "A", Quantity = 1 },
                    new EntryModel { Id = "B", Quantity = 1 }
                ]
            }
        };

        _ordersRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        _productsRepo.Setup(r => r.GetAllAsync()).ReturnsAsync([
            new Product { Id = "A", Name = "Alpha" },
            new Product { Id = "B", Name = "Beta" }
        ]);

        var handler = CreateHandler();

        var request = new HotProductRequest
        {
            StartDate = new DateOnly(2024, 1, 1),
            EndDate = new DateOnly(2024, 1, 5)
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("A", result!.Id);
        Assert.Equal("Alpha", result.Name);
    }
}