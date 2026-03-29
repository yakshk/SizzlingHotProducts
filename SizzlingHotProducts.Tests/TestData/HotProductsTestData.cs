using Application.DTOs;
using Application.Enums;

namespace SizzlingHotProducts.Tests.TestData;

public static class HotProductsTestData
{
    public static HotProductDto SampleHotProductDto()
    {
        return new HotProductDto
        {
            Id = "P1",
            Name = "Cool product",
            Quantity = 5,
        };
    }
    
    public static OrderDto SampleOrderDto()
    {
        return new OrderDto
        {
            CustomerId = "C1",
            Date = new DateOnly(2021, 07, 19),
            OrderId = "O30",
            Status = OrderEnum.OrderStatus.Completed,
            Entries = [
                new EntryDto
                {
                    Id = "P1",
                    Quantity = 3
                }
            ]
        };
    }
}