using Application.DTOs;
using Application.Requests;
using MediatR;
using Moq;
using SizzlingHotProducts.Tests.TestData;
using SizzlingHotProductsApi.Controllers;
using Xunit;

namespace SizzlingHotProducts.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _controller = new ProductsController(_mediator.Object);
    }

    [Fact]
    public async Task GetAll_SendsGetAllProductsRequest()
    {
        // Arrange
        var expected = new List<ProductDto>();
        _mediator
            .Setup(m => m.Send(It.IsAny<GetAllProductsRequest>(), CancellationToken.None))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.GetAll();

        // Assert
        _mediator.Verify(
            m => m.Send(It.Is<GetAllProductsRequest>(r => r != null!), CancellationToken.None),
            Times.Once);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetAll_ReturnsListFromMediator()
    {
        // Arrange
        var expected = new List<ProductDto>
        {
            HotProductsTestData.SampleProductDto()
        };

        _mediator
            .Setup(m => m.Send(It.IsAny<GetAllProductsRequest>(), CancellationToken.None))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAll_CallsMediatorExactlyOnce()
    {
        // Arrange
        _mediator
            .Setup(m => m.Send(It.IsAny<GetAllProductsRequest>(), CancellationToken.None))
            .ReturnsAsync([]);

        // Act
        await _controller.GetAll();

        // Assert
        _mediator.Verify(
            m => m.Send(It.IsAny<GetAllProductsRequest>(), CancellationToken.None),
            Times.Once);
    }
}