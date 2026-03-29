using System.Text.Json;
using Application.Enums;
using Infrastructure.Common;
using Xunit;

namespace Infrastructure.Tests.Common;

public class OrderStatusConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new OrderStatusConverter() }
    };

    [Theory]
    [InlineData("\"Completed\"", OrderEnum.OrderStatus.Completed)]
    [InlineData("\"completed\"", OrderEnum.OrderStatus.Completed)]
    [InlineData("\"COMPLETED\"", OrderEnum.OrderStatus.Completed)]
    [InlineData("\"Cancelled\"", OrderEnum.OrderStatus.Cancelled)]
    public void Read_ParsesValidEnumValues(string json, OrderEnum.OrderStatus expected)
    {
        var result = JsonSerializer.Deserialize<OrderEnum.OrderStatus>(json, _options);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("\"\"", OrderEnum.OrderStatus.Unknown)]
    [InlineData("\"   \"", OrderEnum.OrderStatus.Unknown)]
    [InlineData("null", OrderEnum.OrderStatus.Unknown)]
    [InlineData("\"invalid-value\"", OrderEnum.OrderStatus.Unknown)]
    public void Read_ReturnsUnknownForInvalidOrEmptyValues(string json, OrderEnum.OrderStatus expected)
    {
        var result = JsonSerializer.Deserialize<OrderEnum.OrderStatus>(json, _options);
        Assert.Equal(expected, result);
    }
}