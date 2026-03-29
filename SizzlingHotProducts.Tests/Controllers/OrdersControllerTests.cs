using Application.DTOs;
using Application.Requests;
using MediatR;
using Moq;
using SizzlingHotProducts.Tests.TestData;
using SizzlingHotProductsApi.Controllers;
using Xunit;

namespace SizzlingHotProducts.Tests.Controllers;

public class OrdersControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _controller = new OrdersController(_mediator.Object);
    }

    [Fact]
    public async Task GetAll_SendsGetAllOrdersRequest()
    {
        // Arrange
        var expected = new List<OrderDto>();
        _mediator
            .Setup(m => m.Send(It.IsAny<GetAllOrdersRequest>(), CancellationToken.None))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.GetAll();

        // Assert
        _mediator.Verify(
            m => m.Send(It.Is<GetAllOrdersRequest>(r => r != null!), CancellationToken.None),
            Times.Once);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetAll_ReturnsListFromMediator()
    {
        // Arrange
        var expected = new List<OrderDto>
        {
            HotProductsTestData.SampleOrderDto()
        };

        _mediator
            .Setup(m => m.Send(It.IsAny<GetAllOrdersRequest>(), CancellationToken.None))
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
            .Setup(m => m.Send(It.IsAny<GetAllOrdersRequest>(), CancellationToken.None))
            .ReturnsAsync([]);

        // Act
        await _controller.GetAll();

        // Assert
        _mediator.Verify(
            m => m.Send(It.IsAny<GetAllOrdersRequest>(), CancellationToken.None),
            Times.Once);
    }
}