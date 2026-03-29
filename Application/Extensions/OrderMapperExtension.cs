using Application.DTOs;
using Application.Models;

namespace Application.Extensions;

internal static class OrderMapperExtension
{
    internal static OrderDto ToDto(this Order item)
    {
        return new OrderDto
        {
            CustomerId = item.CustomerId,
            OrderId = item.OrderId,
            Date = item.Date,

            Entries = item.Entries?.Select(e => new EntryDto
            {
                Id = e.Id,
                Quantity = e.Quantity
            }).ToList(),

            Status = item.Status
        };
    }
}