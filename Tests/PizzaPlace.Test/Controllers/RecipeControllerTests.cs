using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using System.Threading.Tasks;

namespace PizzaPlace.Test.Controllers;

[TestClass]
public class RecipeControllerTests
{
    private static RecipeController GetController(Mock<IRecipeRepository> repo) =>
        new(repo.Object);

    [TestMethod]
    public async Task AddRecipe_ReturnsId_FromRepository()
    {
        // Arrange
        var recipe = new PizzaRecipeDto(PizzaRecipeType.RarePizza, new ComparableList<StockDto> { new StockDto(StockType.UnicornDust, 1) }, 5);
        var expectedId = 42L;

        var mockRepo = new Mock<IRecipeRepository>(MockBehavior.Strict);
        mockRepo.Setup(r => r.AddRecipe(It.Is<PizzaRecipeDto>(p => p.RecipeType == recipe.RecipeType)))
                .ReturnsAsync(expectedId);

        var controller = GetController(mockRepo);

        // Act
        var result = await controller.AddRecipe(recipe);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var ok = result as OkObjectResult;
        Assert.AreEqual(expectedId, (long)ok!.Value!);
        mockRepo.VerifyAll();
    }

    [TestMethod]
    public async Task AddRecipe_NullPayload_ReturnsBadRequest()
    {
        // Arrange
        var mockRepo = new Mock<IRecipeRepository>(MockBehavior.Strict);
        var controller = GetController(mockRepo);

        // Act
        var result = await controller.AddRecipe(null);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task UpdateRecipe_UsesRouteId_AndCallsRepository()
    {
        // Arrange
        var routeId = 123L;
        var recipe = new PizzaRecipeDto(PizzaRecipeType.OddPizza, new ComparableList<StockDto> { new StockDto(StockType.Sulphur, 10) }, 10, Id: 0);

        var mockRepo = new Mock<IRecipeRepository>(MockBehavior.Strict);
        mockRepo.Setup(r => r.UpdateRecipe(It.Is<PizzaRecipeDto>(p => p.Id == routeId && p.RecipeType == recipe.RecipeType)))
                .Returns(Task.CompletedTask);

        var controller = GetController(mockRepo);

        // Act
        var result = await controller.UpdateRecipe(routeId, recipe);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
        mockRepo.VerifyAll();
    }

    [TestMethod]
    public async Task UpdateRecipe_NullPayload_ReturnsBadRequest()
    {
        // Arrange
        var mockRepo = new Mock<IRecipeRepository>(MockBehavior.Strict);
        var controller = GetController(mockRepo);

        // Act
        var result = await controller.UpdateRecipe(1, null);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
}
