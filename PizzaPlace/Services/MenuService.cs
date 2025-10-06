using PizzaPlace.Models.Types;

namespace PizzaPlace.Services;

public class MenuService : IMenuService
{
    private readonly Menu _standardMenu;
    private readonly Menu _lunchMenu;

    public MenuService()
    {
        _standardMenu = new Menu(
              "Standard Menu",
              new ComparableList<MenuItem>(new[]
              {
                new MenuItem("Margherita", PizzaRecipeType.StandardPizza, 8.0, false),
                new MenuItem("Pepperoni", PizzaRecipeType.StandardPizza, 9.0, false),
                new MenuItem("Kebab", PizzaRecipeType.StandardPizza, 9.5, false),
                new MenuItem("Veggie", PizzaRecipeType.OddPizza, 8.5, false),
                new MenuItem("BBQ Chicken", PizzaRecipeType.ExtremelyTastyPizza, 10.0, true),
                new MenuItem("Four Cheese", PizzaRecipeType.StandardPizza, 9.5, false),
                new MenuItem("Meat Feast", PizzaRecipeType.ExtremelyTastyPizza, 11.0, false),
                new MenuItem("Seafood", PizzaRecipeType.OddPizza, 12.0, false),
                new MenuItem("Spicy Beef", PizzaRecipeType.StandardPizza, 10.5, true),
                new MenuItem("Mushroom", PizzaRecipeType.OddPizza, 8.5, false),
                new MenuItem("Horse Radish", PizzaRecipeType.HorseRadishPizza, 9.0, false),
                new MenuItem("Custom", PizzaRecipeType.EmptyPizza, 8.0, true)
              })
          );

        _lunchMenu = new Menu(
        "Lunch Menu",
        new ComparableList<MenuItem>(new[]
        {
                new MenuItem("Lunch Deal: Margherita", PizzaRecipeType.StandardPizza, 6.0, false),
                new MenuItem("Lunch Deal: Pepperoni", PizzaRecipeType.StandardPizza, 7.0, false),
                new MenuItem("Lunch Deal: Kebab", PizzaRecipeType.StandardPizza, 7.5, false),
                new MenuItem("Lunch Deal: Veggie", PizzaRecipeType.OddPizza, 6.5, false),
                new MenuItem("Lunch Deal: BBQ Chicken", PizzaRecipeType.ExtremelyTastyPizza, 8.0, true),
                new MenuItem("Lunch Deal: Four Cheese", PizzaRecipeType.StandardPizza, 7.5, false),
                new MenuItem("Lunch Deal: Meat Feast", PizzaRecipeType.ExtremelyTastyPizza, 9.0, false),
                new MenuItem("Lunch Deal: Seafood", PizzaRecipeType.OddPizza, 10.0, false),
                new MenuItem("Lunch Deal: Spicy Beef", PizzaRecipeType.StandardPizza, 8.5, true),
                new MenuItem("Lunch Deal: Mushroom", PizzaRecipeType.OddPizza, 6.5, false),
                new MenuItem("Lunch Deal: Horse Radish", PizzaRecipeType.HorseRadishPizza, 7.0, false),
                new MenuItem("Lunch Deal: Custom", PizzaRecipeType.EmptyPizza, 6.0, true)
        })
    );
    }

    public Menu GetMenu(DateTime utcNowOffset)
    {
        var hour = utcNowOffset.Hour;
        if (hour >= 11 && hour < 15)
            {
            return _lunchMenu;
        }
        return _standardMenu;
    }


    public Menu GetMenu(DateTimeOffset menuDate)
    {
        return GetMenu(menuDate.UtcDateTime);
    }
}