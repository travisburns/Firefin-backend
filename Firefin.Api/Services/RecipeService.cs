using Firefin.Api.Data;
using Firefin.Api.DTOs;
using Firefin.Api.Mapping;
using Firefin.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Firefin.Api.Services;

public class RecipeService : IRecipeService
{
    private readonly FirefinDbContext _db;

    public RecipeService(FirefinDbContext db) => _db = db;

    public async Task<IReadOnlyList<RecipeDto>?> GetForProductAsync(int productId)
    {
        if (!await _db.Products.AnyAsync(p => p.Id == productId))
            return null;

        var recipes = await _db.Recipes
            .Where(r => r.ProductId == productId)
            .Include(r => r.Ingredients)
            .Include(r => r.Batches)
            .OrderByDescending(r => r.Version)
            .ToListAsync();

        return recipes.Select(r => r.ToDto()).ToList();
    }

    public async Task<RecipeDto?> GetByIdAsync(int recipeId)
    {
        var recipe = await _db.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Batches)
            .FirstOrDefaultAsync(r => r.Id == recipeId);
        return recipe?.ToDto();
    }

    public async Task<RecipeDto?> CreateAsync(int productId, CreateRecipeDto dto)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product is null) return null;

        var nextVersion = await _db.Recipes
            .Where(r => r.ProductId == productId)
            .Select(r => (int?)r.Version)
            .MaxAsync() ?? 0;
        nextVersion++;

        // Only one current version per product.
        await _db.Recipes
            .Where(r => r.ProductId == productId && r.IsCurrent)
            .ForEachAsync(r => r.IsCurrent = false);

        var recipe = new Recipe
        {
            ProductId = productId,
            Version = nextVersion,
            IsCurrent = true,
            Notes = dto.Notes,
            Ingredients = dto.Ingredients.Select(i => new RecipeIngredient
            {
                Name = i.Name,
                Grams = i.Grams,
                Notes = i.Notes,
                SortOrder = i.SortOrder
            }).ToList()
        };

        _db.Recipes.Add(recipe);
        product.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return recipe.ToDto();
    }
}
