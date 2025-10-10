using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Services;
using PizzaPlace.Models.Types; // Add this for PizzaRecipeType

namespace PizzaPlace.Controllers;

[Route("api/order")]
public class OrderingController(
    IOrderingService orderingService) : ControllerBase
{
    public class OrderItemDto
    {
        public PizzaRecipeType PizzaType { get; set; }
        public int Amount { get; set; }
    }

    public class PizzaOrderDto
    {
        [System.ComponentModel.DataAnnotations.Required]
        public List<OrderItemDto> RequestedOrder { get; set; } = [];
    }

    [HttpPost]
    public async Task<IActionResult> PlacePizzaOrder([FromBody] PizzaOrderDto pizzaOrderDto)
    {
        try
        {
            var pizzaAmounts = pizzaOrderDto.RequestedOrder
                .Select(x => new PizzaAmount(x.PizzaType, (ushort)x.Amount))
                .ToList();

            var modelOrder = new PizzaOrder(new ComparableList<PizzaAmount>(pizzaAmounts));

            return Ok(new
            {
                pizzas = await orderingService.HandlePizzaOrder(modelOrder),
            });
        }
        catch (PizzaException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
