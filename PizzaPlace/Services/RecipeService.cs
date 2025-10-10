using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;

public class RecipeService(IRecipeRepository recipeRepository) : IRecipeService
{
    public async Task<ComparableList<PizzaRecipeDto>> GetPizzaRecipes(PizzaOrder order)
    {
        List<Models.Types.PizzaRecipeType> pizzaTypes = GetDistinctPizzaTypes(order);

        ComparableList<PizzaRecipeDto> recipes = [];
        foreach (var pizzaType in pizzaTypes)
        {
            recipes.Add(await recipeRepository.GetRecipe(pizzaType));
        }

        return recipes;
    }

    private static List<Models.Types.PizzaRecipeType> GetDistinctPizzaTypes(PizzaOrder order)
    {
        return order.RequestedOrder
                    .Select(x => x.PizzaType)
                    .Distinct()
                    .ToList();
    }
}
