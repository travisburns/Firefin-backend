using Firefin.Api.DTOs;
using Firefin.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Firefin.Api.Controllers;

[ApiController]
[Route("api")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipes;

    public RecipesController(IRecipeService recipes) => _recipes = recipes;

    [HttpGet("products/{productId:int}/recipes")]
    public async Task<ActionResult<IReadOnlyList<RecipeDto>>> GetForProduct(int productId)
    {
        var recipes = await _recipes.GetForProductAsync(productId);
        return recipes is null ? NotFound() : Ok(recipes);
    }

    [HttpGet("recipes/{recipeId:int}")]
    public async Task<ActionResult<RecipeDto>> GetById(int recipeId)
    {
        var recipe = await _recipes.GetByIdAsync(recipeId);
        return recipe is null ? NotFound() : Ok(recipe);
    }

    [HttpPost("products/{productId:int}/recipes")]
    public async Task<ActionResult<RecipeDto>> Create(int productId, CreateRecipeDto dto)
    {
        var created = await _recipes.CreateAsync(productId, dto);
        return created is null
            ? NotFound()
            : CreatedAtAction(nameof(GetById), new { recipeId = created.Id }, created);
    }
}
