using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories;

/// <summary>
/// Producing one line of pizza. 
/// Taking 7 minutes to setup - and then 5 minutes less for every subsequent pizza of the same recipe type to a minimum of 4 minutes.
/// </summary>
public class AssemblyLinePizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
{
    private const int AssemblyLineCapacity = 1;
    public const int SetupTimeMinutes = 7;
    public const int SubsequentPizzaTimeSavingsInMinutes = 5;
    public const int MinimumCookingTimeMinutes = 4;

    protected override int Capacity => AssemblyLineCapacity;

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
    {
        // Keep track of the previous recipe type so we can apply setup time only on type change
        PizzaRecipeType? previousType = null;

        foreach (var (recipe, orderGuid) in recipeOrders)
        {
            var recipeType = recipe.RecipeType;

            // Determine cooking time according to assembly-line rules:
            // - First pizza of a run = recipe cooking time + setup time
            // - Future pizzas of same type = recipe cooking time minus savings, down to minimum of 4 minutes
            int cookingMinutes;
            if (previousType is null || previousType.Value != recipeType)
            {
                cookingMinutes = recipe.CookingTimeMinutes + SetupTimeMinutes;
            }
            else
            {
                cookingMinutes = Math.Max(recipe.CookingTimeMinutes - SubsequentPizzaTimeSavingsInMinutes, MinimumCookingTimeMinutes);
            }

            // Enqueue a task creator that captures this instance so CookPizza/GetPizza use the oven's timeProvider
            _pizzaQueue.Enqueue((MakePizzaLocal(recipe, cookingMinutes), orderGuid));

            previousType = recipeType;
        }

        // Instance-local helper so it can call protected instance methods CookPizza/GetPizza
        Func<Task<Pizza?>> MakePizzaLocal(PizzaRecipeDto r, int minutes) => async () =>
        {
            await CookPizza(minutes); // uses this oven's TimeProvider (FakeTimeProvider in tests)
            return GetPizza(r.RecipeType);
        };
    }
}
