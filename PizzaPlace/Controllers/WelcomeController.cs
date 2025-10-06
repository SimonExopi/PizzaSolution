using Microsoft.AspNetCore.Mvc;

namespace PizzaPlace.Controllers;

[Route("api/welcome")]
public class WelcomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Greet()
    {
        Console.WriteLine("Is Pizza yummy?.");

        return Ok("Pizza yummy 🍕👌.");
    }
}
