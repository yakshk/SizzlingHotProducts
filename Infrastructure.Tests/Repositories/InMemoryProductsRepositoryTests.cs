using Infrastructure.Repositories;
using Infrastructure.Tests.TestData;
using Xunit;

namespace Infrastructure.Tests.Repositories;

public class InMemoryProductsRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsNewListEachCall()
    {
        // Arrange
        var repo = new InMemoryProductsRepository();

        // Act
        var list1 = await repo.GetAllAsync();
        var list2 = await repo.GetAllAsync();

        // Assert
        Assert.NotSame(list1, list2);
    }

    [Fact]
    public async Task ModifyingReturnedList_DoesNotAffectRepository()
    {
        // Arrange
        var repo = new InMemoryProductsRepository();

        var product = ReposTestData.SampleProduct();
        product.Name = "NEW";

        // Act
        var list = await repo.GetAllAsync();
        list.Add(product);

        var resultAfter = await repo.GetAllAsync();

        // Assert
        Assert.Equal(list.Count - 1, resultAfter.Count);
    }
}