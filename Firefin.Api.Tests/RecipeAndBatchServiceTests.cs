using Firefin.Api.DTOs;
using Firefin.Api.Models;
using Firefin.Api.Services;
using Xunit;

namespace Firefin.Api.Tests;

public class RecipeAndBatchServiceTests
{
    [Fact]
    public async Task Recipe_versions_increment_and_only_latest_is_current()
    {
        await using var db = TestDb.Create();
        var products = new ProductService(db);
        var recipes = new RecipeService(db);

        var product = await products.CreateAsync(
            new CreateProductDto("Blue Flame", ProductType.Sauce, 3, null, null));

        var v1 = await recipes.CreateAsync(product.Id,
            new CreateRecipeDto("v1", new[] { new CreateRecipeIngredientDto("Blueberry", 200m, null, 1) }));
        var v2 = await recipes.CreateAsync(product.Id,
            new CreateRecipeDto("v2", new[] { new CreateRecipeIngredientDto("Blueberry", 180m, null, 1) }));

        Assert.Equal(1, v1!.Version);
        Assert.Equal(2, v2!.Version);

        var all = await recipes.GetForProductAsync(product.Id);
        Assert.NotNull(all);
        Assert.True(all!.Single(r => r.Version == 2).IsCurrent);
        Assert.False(all.Single(r => r.Version == 1).IsCurrent);
    }

    [Fact]
    public async Task Batch_numbers_increment_per_recipe()
    {
        await using var db = TestDb.Create();
        var products = new ProductService(db);
        var recipes = new RecipeService(db);
        var batches = new BatchService(db);

        var product = await products.CreateAsync(
            new CreateProductDto("Blue Flame", ProductType.Sauce, 3, null, null));
        var recipe = await recipes.CreateAsync(product.Id,
            new CreateRecipeDto(null, Array.Empty<CreateRecipeIngredientDto>()));

        var b1 = await batches.CreateAsync(recipe!.Id,
            new CreateBatchDto(DateOnly.FromDateTime(DateTime.UtcNow), "Travis", 3, "baseline", null, null));
        var b2 = await batches.CreateAsync(recipe.Id,
            new CreateBatchDto(DateOnly.FromDateTime(DateTime.UtcNow), "Travis", 4, "better", null,
                new[] { new CreateBatchNoteDto(NoteCategory.Heat, "balanced", null, 2) }));

        Assert.Equal(1, b1!.BatchNumber);
        Assert.Equal(2, b2!.BatchNumber);
        Assert.Single(b2.Notes);
    }

    [Fact]
    public async Task CreateBatch_returns_null_for_missing_recipe()
    {
        await using var db = TestDb.Create();
        var batches = new BatchService(db);

        var result = await batches.CreateAsync(999,
            new CreateBatchDto(DateOnly.FromDateTime(DateTime.UtcNow), null, null, null, null, null));

        Assert.Null(result);
    }
}
