using System.Text.Json;
using Infrastructure.Common;
using Xunit;

namespace Infrastructure.Tests.Common;

public class DateOnlyConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new DateOnlyConverter() }
    };

    [Fact]
    public void Read_ParsesValidDate()
    {
        // Arrange
        const string json = "\"21/07/2021\"";

        // Act
        var result = JsonSerializer.Deserialize<DateOnly>(json, _options);

        // Assert
        Assert.Equal(new DateOnly(2021, 7, 21), result);
    }

    [Fact]
    public void Read_ThrowsOnInvalidFormat()
    {
        // Arrange
        const string json = "\"2021-07-21\"";

        // Act + Assert
        Assert.Throws<FormatException>(() =>
            JsonSerializer.Deserialize<DateOnly>(json, _options));
    }
}