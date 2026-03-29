using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Enums;

namespace Infrastructure.Common;

public class OrderStatusConverter : JsonConverter<OrderEnum.OrderStatus>
{
    public override OrderEnum.OrderStatus Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
            return OrderEnum.OrderStatus.Unknown;

        return Enum.TryParse<OrderEnum.OrderStatus>(value, true, out var result)
            ? result
            : OrderEnum.OrderStatus.Unknown;
    }

    public override void Write(Utf8JsonWriter writer, OrderEnum.OrderStatus value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}