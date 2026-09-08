using System.ComponentModel.DataAnnotations;

namespace Firefin.Api.Models;

/// <summary>
/// A versioned formulation of a product. A product accumulates recipe versions
/// as it is iterated; exactly one version is marked current at a time.
/// </summary>
public class Recipe
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    /// <summary>Monotonic version number within the product, starting at 1.</summary>
    public int Version { get; set; }

    /// <summary>Whether this is the active working version for the product.</summary>
    public bool IsCurrent { get; set; }

    [MaxLength(4000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<RecipeIngredient> Ingredients { get; set; } = new();
    public List<Batch> Batches { get; set; } = new();
}
