using Firefin.Api.DTOs;

namespace Firefin.Api.Services;

public interface IRecipeService
{
    /// <summary>Recipe versions for a product, newest version first. Null if the product does not exist.</summary>
    Task<IReadOnlyList<RecipeDto>?> GetForProductAsync(int productId);

    Task<RecipeDto?> GetByIdAsync(int recipeId);

    /// <summary>Adds a new version to a product and makes it current. Null if the product does not exist.</summary>
    Task<RecipeDto?> CreateAsync(int productId, CreateRecipeDto dto);
}
