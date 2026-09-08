using System.ComponentModel.DataAnnotations;

namespace Firefin.Api.Models;

/// <summary>
/// A customer order. Totals are computed server-side from the line items, never
/// trusted from the client.
/// </summary>
public class Order
{
    public int Id { get; set; }

    /// <summary>Human-friendly reference, e.g. "FF-260908-4KQ2".</summary>
    [Required, MaxLength(32)]
    public string OrderNumber { get; set; } = string.Empty;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Required, MaxLength(120)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string ShippingLine1 { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ShippingLine2 { get; set; }

    [Required, MaxLength(120)]
    public string ShippingCity { get; set; } = string.Empty;

    [MaxLength(120)]
    public string? ShippingRegion { get; set; }

    [Required, MaxLength(20)]
    public string ShippingPostalCode { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string ShippingCountry { get; set; } = string.Empty;

    public decimal Subtotal { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}
