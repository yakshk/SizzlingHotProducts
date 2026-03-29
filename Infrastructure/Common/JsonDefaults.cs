using System.Text.Json;

namespace Infrastructure.Common;

public static class JsonDefaults
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new DateOnlyConverter(),
            new OrderStatusConverter()
        }
    };
}