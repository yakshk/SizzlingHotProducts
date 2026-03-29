using Application.Extensions;
using Application.Models;
using Application.Tests.TestData;
using Xunit;

namespace Application.Tests.Extensions;

public class ProductMapperExtensionTests
{
    [Fact]
    public void ToDto_MapsScalarPropertiesCorrectly()
    {
        // Arrange
        var product = ObjectTestData.SampleProduct();

        // Act
        var dto = product.ToDto();

        // Assert
        Assert.Equal(product.Id, dto.Id);
        Assert.Equal(product.Name, dto.Name);
    }

    [Fact]
    public void ToDto_ReturnsNewInstance()
    {
        // Arrange
        var product = ObjectTestData.SampleProduct();

        // Act
        var dto = product.ToDto();

        // Assert
        Assert.NotSame(product, dto);
    }

    [Fact]
    public void ToDto_HandlesTypicalTestData()
    {
        // Arrange
        var product = new Product
        {
            Id = "ABC-001",
            Name = "Premium Coffee Beans"
        };

        // Act
        var dto = product.ToDto();

        // Assert
        Assert.Equal("ABC-001", dto.Id);
        Assert.Equal("Premium Coffee Beans", dto.Name);
    }
}