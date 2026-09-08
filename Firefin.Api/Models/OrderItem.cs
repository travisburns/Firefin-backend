using System.ComponentModel.DataAnnotations;

namespace Firefin.Api.Models;

/// <summary>
/// A single line on an order. May reference a catalog product (single item) or
/// stand alone (a Build Your Freezer box, which has no single product id).
/// </summary>
public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    /// <summary>Catalog product id when this line is a single product; null for a freezer box.</summary>
    public int? ProductId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Subtitle { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal LineTotal { get; set; }
}
