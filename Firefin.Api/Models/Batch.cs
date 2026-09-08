using System.ComponentModel.DataAnnotations;

namespace Firefin.Api.Models;

/// <summary>
/// One physical attempt at making a recipe. Batches are how a recipe is proven
/// repeatable: each carries a number, a verdict, a rating, and structured notes
/// on what worked and what did not.
/// </summary>
public class Batch
{
    public int Id { get; set; }

    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }

    /// <summary>Sequential batch number within the recipe, starting at 1.</summary>
    public int BatchNumber { get; set; }

    public DateOnly MadeOn { get; set; }

    [MaxLength(120)]
    public string? MadeBy { get; set; }

    /// <summary>Overall subjective quality on a 1-5 scale; null until tasted.</summary>
    public int? Rating { get; set; }

    /// <summary>Short verdict, e.g. "keep direction", "too acidic", "scrap".</summary>
    [MaxLength(200)]
    public string? Verdict { get; set; }

    [MaxLength(4000)]
    public string? Summary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<BatchNote> Notes { get; set; } = new();
}
