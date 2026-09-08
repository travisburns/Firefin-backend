using System.ComponentModel.DataAnnotations;

namespace Firefin.Api.Models;

/// <summary>
/// A Firefin product identity (a meal, sauce, or Fire Drop). This is the shared
/// backbone between the R&amp;D Lab and the eventual customer catalog: the Lab
/// develops it through recipes and batches; once Locked it can be sold.
/// </summary>
public class Product
{
    public int Id { get; set; }

    /// <summary>URL-friendly identifier, e.g. "blue-flame".</summary>
    [Required, MaxLength(120)]
    public string Slug { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    public ProductType Type { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Concept;

    /// <summary>Working heat rating on a 1-5 scale; null until decided.</summary>
    public int? HeatLevel { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    /// <summary>Working target retail price; unit economics are validated in a service, not here.</summary>
    public decimal? TargetPrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<Recipe> Recipes { get; set; } = new();
}
