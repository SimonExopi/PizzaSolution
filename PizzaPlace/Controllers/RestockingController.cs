using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Controllers;

[Route("api/restocking")]
public class RestockingController(IStockRepository stockRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Restock([FromBody] ComparableList<StockDto> stock)
    {
        if (stock is null || stock.Count == 0)
            return BadRequest("Request body must contain at least one stock item.");

        var results = new List<StockDto>(stock.Count);
        foreach (var item in stock)
        {
            if (item is null)
                continue;

            var added = await stockRepository.AddToStock(item);
            if (added is null)
            {
                // If repository fails to return the added item treat as server error.
                return StatusCode(500, $"Failed to add stock for {item.StockType}");
            }

            results.Add(added);
        }

        return Ok(new ComparableList<StockDto>(results));
    }
}
