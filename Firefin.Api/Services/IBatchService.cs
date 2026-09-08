using Firefin.Api.DTOs;

namespace Firefin.Api.Services;

public interface IBatchService
{
    /// <summary>Batches for a recipe, newest batch first. Null if the recipe does not exist.</summary>
    Task<IReadOnlyList<BatchDto>?> GetForRecipeAsync(int recipeId);

    Task<BatchDto?> GetByIdAsync(int batchId);

    /// <summary>Adds a batch to a recipe with the next batch number. Null if the recipe does not exist.</summary>
    Task<BatchDto?> CreateAsync(int recipeId, CreateBatchDto dto);
}
