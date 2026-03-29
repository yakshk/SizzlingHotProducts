using Application.Enums;
using Application.Models;

namespace Application.Tests.TestData;

public static class ObjectTestData
{
    public static Order SampleOrder()
    {
        return new Order
        {
            CustomerId = "C1",
            Date = new DateOnly(2021, 07, 19),
            OrderId = "O30",
            Status = OrderEnum.OrderStatus.Completed,
            Entries = [
                new EntryModel
                {
                    Id = "P1",
                    Quantity = 3
                }
            ]
        };
    }

    public static Product SampleProduct()
    {
        return new Product
        {
            Id = "P123",
            Name = "Test Product"
        };
    }
}