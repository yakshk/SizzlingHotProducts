using Application.Enums;

namespace Application.DTOs;

public class OrderDto
{
    public required string OrderId { get; set; }

    public required string CustomerId { get; set; }

    public List<EntryDto>? Entries { get; set; }

    public required DateOnly Date { get; set; }
    public required OrderEnum.OrderStatus Status { get; set; }
}

public class EntryDto
{
    public required string Id { get; set; } // Product ID
    public required int Quantity { get; set; }
}