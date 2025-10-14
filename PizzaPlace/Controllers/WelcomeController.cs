using Microsoft.AspNetCore.Mvc;

namespace PizzaPlace.Controllers;

[Route("api/welcome")]
public class WelcomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Greet()
    {
        Console.WriteLine("Velkommen alle 4");

        return Ok("Automatisk pizza bestilling 4");
    }
}
