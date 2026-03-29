using Application.Enums;
using Application.Handlers;
using Application.Interfaces;
using Application.Models;
using Application.Requests;
using Moq;
using Xunit;

namespace Application.Tests.Handlers;

public class GetAllOrdersHandlerTests
{
    private readonly Mock<IOrdersRepository> _repo = new();
    private readonly GetAllOrdersHandler _handler;

    public GetAllOrdersHandlerTests()
    {
        _handler = new GetAllOrdersHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_CallsRepositoryExactlyOnce()
    {
        // Arrange
        _repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync([]);

        // Act
        await _handler.Handle(new GetAllOrdersRequest(), CancellationToken.None);

        // Assert
        _repo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenRepositoryReturnsEmpty()
    {
        // Arrange
        _repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(new GetAllOrdersRequest(), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_MapsOrdersToDtos()
    {
        // Arrange
        var orders = new List<Order>
        {
            new()
            {
                CustomerId = "C1",
                OrderId = "O1",
                Date = new DateOnly(2024, 1, 5),
                Status = OrderEnum.OrderStatus.Completed,
                Entries = [new EntryModel { Id = "P1", Quantity = 2 }]
            }
        };

        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _handler.Handle(new GetAllOrdersRequest(), CancellationToken.None);

        // Assert
        Assert.Single(result);

        var dto = result[0];
        Assert.Equal("C1", dto.CustomerId);
        Assert.Equal("O1", dto.OrderId);
        Assert.Equal(new DateOnly(2024, 1, 5), dto.Date);
        Assert.Equal(OrderEnum.OrderStatus.Completed, dto.Status);

        Assert.Single(dto.Entries!);
        Assert.Equal("P1", dto.Entries![0].Id);
        Assert.Equal(2, dto.Entries[0].Quantity);
    }

    [Fact]
    public async Task Handle_ReturnsNewList_NotRepositoryList()
    {
        // Arrange
        var orders = new List<Order>();
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _handler.Handle(new GetAllOrdersRequest(), CancellationToken.None);

        // Assert
        Assert.NotSame(orders, result);
    }

    [Fact]
    public async Task Handle_MapsMultipleOrdersCorrectly()
    {
        // Arrange
        var orders = new List<Order>
        {
            new() { CustomerId = "A", OrderId = "1", Date = new DateOnly(2024, 1, 1), Status = OrderEnum.OrderStatus.Completed },
            new() { CustomerId = "B", OrderId = "2", Date = new DateOnly(2024, 1, 2), Status = OrderEnum.OrderStatus.Cancelled }
        };

        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _handler.Handle(new GetAllOrdersRequest(), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("A", result[0].CustomerId);
        Assert.Equal("B", result[1].CustomerId);
    }
}