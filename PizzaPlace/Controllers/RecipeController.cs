using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Controllers;

[Route("api/recipes")]
public class RecipeController(IRecipeRepository recipeRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddRecipe([FromBody] PizzaRecipeDto? recipe)
    {
        if (recipe is null)
            return BadRequest("Request body must contain a recipe.");

        try
        {
            var id = await recipeRepository.AddRecipe(recipe);
            return Ok(id);
        }
        catch (PizzaException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred while adding the recipe.");
        }
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateRecipe(long id, [FromBody] PizzaRecipeDto? recipe)
    {
        if (recipe is null)
            return BadRequest("Request body must contain a recipe.");

        // Ensure route id is authoritative
        var recipeToUpdate = recipe with { Id = id };

        try
        {
            await recipeRepository.UpdateRecipe(recipeToUpdate);
            return Ok();
        }
        catch (PizzaException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred while updating the recipe.");
        }
    }
}
