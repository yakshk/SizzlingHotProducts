using Application.Enums;

namespace Application.Models;

public class Order
{
    public required string OrderId { get; set; }

    public required string CustomerId { get; set; }

    public List<EntryModel>? Entries { get; set; }

    public required DateOnly Date { get; set; }
    public required OrderEnum.OrderStatus Status { get; set; }
}

public class EntryModel
{
    public required string Id { get; set; } // Product ID
    public required int Quantity { get; set; }
}