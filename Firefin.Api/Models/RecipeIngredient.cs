using System.ComponentModel.DataAnnotations;

namespace Firefin.Api.Models;

/// <summary>
/// A single line of a gram-weight formula. The blueprint requires recipes to be
/// written as exact gram weights, not cooking descriptions, so this is the unit
/// of a formulation.
/// </summary>
public class RecipeIngredient
{
    public int Id { get; set; }

    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Quantity in grams for this line at the recipe's stated yield.</summary>
    public decimal Grams { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>Display order within the formula.</summary>
    public int SortOrder { get; set; }
}
