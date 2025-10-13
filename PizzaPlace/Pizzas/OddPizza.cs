using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;

public record OddPizza : Pizza
{
    public OddPizza() : base(PizzaRecipeType.OddPizza)
    {
    }
}