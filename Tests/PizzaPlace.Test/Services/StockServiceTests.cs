using Moq;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;
using System.Threading.Tasks;

namespace PizzaPlace.Test.Services;

[TestClass]
public class StockServiceTests
{
    private static IStockService GetService(Mock<IStockRepository> stockRepository) => new StockService(stockRepository.Object);

    private static ComparableList<StockDto> GetStandardStock() =>
        [
            new StockDto(StockType.Dough, 100),
            new StockDto(StockType.Tomatoes, 0),
            new StockDto(StockType.Bacon, -100),
        ];

    [TestMethod]
    public async Task StockTest()
    {
        // Arrange
        var expectedStock = GetStandardStock();
        var stockRepository = new Mock<IStockRepository>(MockBehavior.Strict);
        stockRepository
            .Setup(repo => repo.GetStock(It.IsAny<StockType>()))
            .ReturnsAsync((StockType type) => expectedStock.FirstOrDefault(s => s.StockType == type) ?? new StockDto(type, 0));

        var stockService = GetService(stockRepository);

        // Dummy order and recipes
        var dummyOrder = new PizzaOrder(new ComparableList<PizzaAmount>());
        var dummyRecipes = new ComparableList<PizzaRecipeDto>();

        // Act
        var actualStock = await stockService.GetStock(dummyOrder, dummyRecipes);

        // Assert
        Assert.IsNotNull(actualStock, "GetStock should not return null");
        Assert.IsTrue(expectedStock.Equals(actualStock));
    }
}
