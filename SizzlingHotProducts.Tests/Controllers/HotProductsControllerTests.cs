using Application.Constants;
using Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SizzlingHotProducts.Tests.TestData;
using SizzlingHotProductsApi.Controllers;
using Xunit;

namespace SizzlingHotProducts.Tests.Controllers;

public class HotProductsControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly HotProductsController _controller;

    public HotProductsControllerTests()
    {
        _controller = new HotProductsController(_mediator.Object);
    }

    [Fact]
    public async Task HotProductToday_SendsCorrectRequest()
    {
        // Arrange
        var expected = HotProductsTestData.SampleHotProductDto();
        _mediator
            .Setup(m => m.Send(It.IsAny<HotProductRequest>(), CancellationToken.None))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.HotProductToday();

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<HotProductRequest>(r =>
                r.StartDate == BusinessConstants.Today &&
                r.EndDate == BusinessConstants.Today),
            CancellationToken.None));

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task HotProductPastThreeDays_SendsCorrectRequest()
    {
        // Arrange
        var expected = HotProductsTestData.SampleHotProductDto();
        _mediator
            .Setup(m => m.Send(It.IsAny<HotProductRequest>(), CancellationToken.None))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.HotProductPastThreeDays();

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<HotProductRequest>(r =>
                r.StartDate == BusinessConstants.Today.AddDays(-2) &&
                r.EndDate == BusinessConstants.Today),
            CancellationToken.None));

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DateRange_ForwardsRequestToMediator()
    {
        // Arrange
        var request = new HotProductRequest
        {
            StartDate = new DateOnly(2024, 01, 01),
            EndDate = new DateOnly(2024, 01, 05)
        };

        var expected = HotProductsTestData.SampleHotProductDto();
        _mediator
            .Setup(m => m.Send(request, CancellationToken.None))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.DateRange(request);

        // Assert
        _mediator.Verify(m => m.Send(request, CancellationToken.None));
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task HotProductToday_ReturnsOkWithDto()
    {
        // Arrange
        var expected = HotProductsTestData.SampleHotProductDto();
        _mediator.Setup(m => m.Send(It.IsAny<HotProductRequest>(), CancellationToken.None))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.HotProductToday();

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public async Task HotProductPastThreeDays_CallsMediatorOnce()
    {
        // Arrange
        _mediator.Setup(m => m.Send(It.IsAny<HotProductRequest>(), CancellationToken.None))
            .ReturnsAsync(HotProductsTestData.SampleHotProductDto());

        // Act
        await _controller.HotProductPastThreeDays();

        // Assert
        _mediator.Verify(m => m.Send(It.IsAny<HotProductRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DateRange_PassesSameRequestInstance()
    {
        // Arrange
        var request = new HotProductRequest
        {
            StartDate = new DateOnly(2024, 1, 1),
            EndDate = new DateOnly(2024, 1, 5)
        };

        var expected = HotProductsTestData.SampleHotProductDto();
        _mediator.Setup(m => m.Send(request, CancellationToken.None)).ReturnsAsync(expected);

        // Act
        var result = await _controller.DateRange(request);

        // Assert
        _mediator.Verify(m => m.Send(request, CancellationToken.None));
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public async Task HotProductPastThreeDays_UsesCorrectDateRange()
    {
        // Arrange
        var expected = HotProductsTestData.SampleHotProductDto();
        _mediator.Setup(m => m.Send(It.IsAny<HotProductRequest>(), CancellationToken.None))
            .ReturnsAsync(expected);

        // Act
        await _controller.HotProductPastThreeDays();

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<HotProductRequest>(r =>
                r.StartDate == BusinessConstants.Today.AddDays(-2) &&
                r.EndDate == BusinessConstants.Today),
            CancellationToken.None));
    }
}