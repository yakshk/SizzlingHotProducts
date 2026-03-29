using Application.Handlers;
using Application.Interfaces;
using Application.Models;
using Application.Requests;
using Moq;
using Xunit;

namespace Application.Tests.Handlers;

public class GetAllProductsHandlerTests
{
    private readonly Mock<IProductsRepository> _repo = new();
    private readonly GetAllProductsHandler _handler;

    public GetAllProductsHandlerTests()
    {
        _handler = new GetAllProductsHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_CallsRepositoryExactlyOnce()
    {
        // Arrange
        _repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync([]);

        // Act
        await _handler.Handle(new GetAllProductsRequest(), CancellationToken.None);

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
        var result = await _handler.Handle(new GetAllProductsRequest(), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_MapsProductsToDtos()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = "P1", Name = "Coffee" }
        };

        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(new GetAllProductsRequest(), CancellationToken.None);

        // Assert
        Assert.Single(result);

        var dto = result[0];
        Assert.Equal("P1", dto.Id);
        Assert.Equal("Coffee", dto.Name);
    }

    [Fact]
    public async Task Handle_ReturnsNewList_NotRepositoryList()
    {
        // Arrange
        var products = new List<Product>();
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(new GetAllProductsRequest(), CancellationToken.None);

        // Assert
        Assert.NotSame(products, result);
    }

    [Fact]
    public async Task Handle_MapsMultipleProductsCorrectly()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = "A", Name = "Item A" },
            new() { Id = "B", Name = "Item B" }
        };

        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(new GetAllProductsRequest(), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("A", result[0].Id);
        Assert.Equal("B", result[1].Id);
    }
}