namespace Firefin.Api.DTOs;

public record RecipeIngredientDto(
    int Id,
    string Name,
    decimal Grams,
    string? Notes,
    int SortOrder);

public record RecipeDto(
    int Id,
    int ProductId,
    int Version,
    bool IsCurrent,
    string? Notes,
    DateTime CreatedAt,
    IReadOnlyList<RecipeIngredientDto> Ingredients,
    int BatchCount);

public record CreateRecipeIngredientDto(
    string Name,
    decimal Grams,
    string? Notes,
    int SortOrder);

/// <summary>
/// Create a new recipe version for a product. The service assigns the next
/// version number and manages the IsCurrent flag.
/// </summary>
public record CreateRecipeDto(
    string? Notes,
    IReadOnlyList<CreateRecipeIngredientDto> Ingredients);
