using Infrastructure.Repositories;
using Infrastructure.Tests.TestData;
using Xunit;

namespace Infrastructure.Tests.Repositories;

public class InMemoryOrdersRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsNewListEachCall()
    {
        // Arrange
        var repo = new InMemoryOrdersRepository();

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
        var repo = new InMemoryOrdersRepository();

        var order = ReposTestData.SampleOrder();
        order.Entries![0].Id = "NEW";

        // Act
        var list = await repo.GetAllAsync();
        list.Add(order);

        var resultAfter = await repo.GetAllAsync();

        // Assert
        Assert.Equal(list.Count - 1, resultAfter.Count);
    }
}