using Application.Enums;
using Application.Extensions;
using Application.Models;
using Application.Tests.TestData;
using Xunit;

namespace Application.Tests.Extensions;

public class OrderMapperExtensionTests
{
    [Fact]
    public void ToDto_MapsScalarPropertiesCorrectly()
    {
        // Arrange
        var order = ObjectTestData.SampleOrder();

        // Act
        var dto = order.ToDto();

        // Assert
        Assert.Equal(order.CustomerId, dto.CustomerId);
        Assert.Equal(order.OrderId, dto.OrderId);
        Assert.Equal(order.Date, dto.Date);
        Assert.Equal(order.Status, dto.Status);
    }

    [Fact]
    public void ToDto_MapsEntriesCorrectly()
    {
        // Arrange
        var order = new Order
        {
            CustomerId = "C1",
            OrderId = "O1",
            Date = new DateOnly(2024, 1, 5),
            Status = OrderEnum.OrderStatus.Completed,
            Entries =
            [
                new EntryModel { Id = "P1", Quantity = 2 },
                new EntryModel { Id = "P2", Quantity = 5 }
            ]
        };

        // Act
        var dto = order.ToDto();

        // Assert
        Assert.Equal(2, dto.Entries!.Count);

        Assert.Equal("P1", dto.Entries[0].Id);
        Assert.Equal(2, dto.Entries[0].Quantity);

        Assert.Equal("P2", dto.Entries[1].Id);
        Assert.Equal(5, dto.Entries[1].Quantity);
    }

    [Fact]
    public void ToDto_CreatesNewEntryList_NotReferenceCopy()
    {
        // Arrange
        var order = ObjectTestData.SampleOrder();
        order.Entries = [new EntryModel { Id = "P1", Quantity = 1 }];

        // Act
        var dto = order.ToDto();

        // Assert
        Assert.NotSame(order.Entries, dto.Entries);
    }

    [Fact]
    public void ToDto_CreatesNewEntryObjects_NotReferenceCopy()
    {
        // Arrange
        var entry = new EntryModel { Id = "P1", Quantity = 1 };
        var order = ObjectTestData.SampleOrder();
        order.Entries = [entry];

        // Act
        var dto = order.ToDto();

        // Assert
        Assert.NotSame(entry, dto.Entries![0]);
    }

    [Fact]
    public void ToDto_HandlesNullEntries()
    {
        // Arrange
        var order = new Order
        {
            CustomerId = "C1",
            OrderId = "O1",
            Date = new DateOnly(2024, 1, 5),
            Status = OrderEnum.OrderStatus.Completed,
            Entries = null
        };

        // Act
        var dto = order.ToDto();

        // Assert
        Assert.Null(dto.Entries);
    }
}