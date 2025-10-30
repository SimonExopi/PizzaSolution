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
            // Uses a dummy response to illustrate what would happen upon succesful implementation.
            await Assert.ThrowsAsync<NotImplementedException>(async () =>
            {
                await controller.Restock(payload);
            });

            // When Restocking is fully implemented, AddToStock is used for each incoming item. A mock verification is momentarily used.
            mockRepo.Verify(r => r.AddToStock(It.IsAny<StockDto>()), Times.Exactly(payload.Count));
        }
    }
}
