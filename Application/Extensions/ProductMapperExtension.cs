using Application.DTOs;
using Application.Models;

namespace Application.Extensions;

internal static class ProductMapperExtension
{
    internal static ProductDto ToDto(this Product item)
    {
        return new ProductDto
        {
            Id = item.Id,
            Name = item.Name
        };
    }
}