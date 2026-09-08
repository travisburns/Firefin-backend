using Firefin.Api.Models;

namespace Firefin.Api.DTOs;

public record OrderItemDto(
    int Id,
    int? ProductId,
    string Title,
    string? Subtitle,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);

public record OrderDto(
    int Id,
    string OrderNumber,
    OrderStatus Status,
    string CustomerName,
    string CustomerEmail,
    string ShippingLine1,
    string? ShippingLine2,
    string ShippingCity,
    string? ShippingRegion,
    string ShippingPostalCode,
    string ShippingCountry,
    decimal Subtotal,
    DateTime CreatedAt,
    DateTime? PaidAt,
    IReadOnlyList<OrderItemDto> Items);

/// <summary>
/// One requested line. If <see cref="ProductSlug"/> is set it is treated as a
/// catalog product and re-priced server-side; otherwise it is a standalone line
/// (e.g. a freezer box) and the provided unit price is used.
/// </summary>
public record CreateOrderItemDto(
    string? ProductSlug,
    string Title,
    string? Subtitle,
    decimal UnitPrice,
    int Quantity);

public record CreateOrderDto(
    string CustomerName,
    string CustomerEmail,
    string ShippingLine1,
    string? ShippingLine2,
    string ShippingCity,
    string? ShippingRegion,
    string ShippingPostalCode,
    string ShippingCountry,
    IReadOnlyList<CreateOrderItemDto> Items);
