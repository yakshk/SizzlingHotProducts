using Application.DTOs;

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
}