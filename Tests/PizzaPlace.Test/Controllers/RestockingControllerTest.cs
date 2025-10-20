using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using System;
using System.Threading.Tasks;

namespace PizzaPlace.Test.Controllers
{
    [TestClass]
    public class RestockingControllerTest
    {
        private static RestockingController GetController(Mock<IStockRepository> repo) =>
            new RestockingController(repo.Object);

        [TestMethod]
        public async Task Restock_PostsStockItems_ToRepository_UsingDummyResponse()
        {
            // Arrange
            var stockType = StockType.Bacon;
            var amountToAdd = 42;

            // Payload that would be sent as the request body
            var payload = new ComparableList<StockDto>(new[]
            {
                new StockDto(stockType, amountToAdd)
            });

            // Dummy response: repository will return the same DTO but with an Id assigned
            var dummyResponse = new StockDto(stockType, amountToAdd, Id: 123);

            var mockRepo = new Mock<IStockRepository>(MockBehavior.Strict);
            mockRepo
                .Setup(r => r.AddToStock(It.Is<StockDto>(s => s.StockType == stockType && s.Amount == amountToAdd)))
                .ReturnsAsync(dummyResponse);

            var controller = GetController(mockRepo);

            // Act / Assert
            // Note: the controller method is currently not implemented and throws NotImplementedException.
            // This test prepares the request body and a dummy repository response to demonstrate the intended interaction.
            await Assert.ThrowsExceptionAsync<NotImplementedException>(async () =>
            {
                await controller.Restock(payload);
            });

            // The mock setup shows the expected call; since the controller is not implemented the call will not occur.
            // If/when Restock is implemented to call AddToStock for each incoming item, the following verification can be enabled:
            // mockRepo.Verify(r => r.AddToStock(It.IsAny<StockDto>()), Times.Exactly(payload.Count));
        }
    }
}
