using Firefin.Api.Models;

namespace Firefin.Api.DTOs;

/// <summary>Product as returned to clients (Lab and, later, storefront).</summary>
public record ProductDto(
    int Id,
    string Slug,
    string Name,
    ProductType Type,
    ProductStatus Status,
    int? HeatLevel,
    string? Description,
    decimal? TargetPrice,
    int RecipeCount,
    DateTime CreatedAt,
    DateTime UpdatedAt);

/// <summary>Payload to create a new product identity.</summary>
public record CreateProductDto(
    string Name,
    ProductType Type,
    int? HeatLevel,
    string? Description,
    decimal? TargetPrice);

/// <summary>Payload to update an existing product's editable fields.</summary>
public record UpdateProductDto(
    string Name,
    ProductStatus Status,
    int? HeatLevel,
    string? Description,
    decimal? TargetPrice);
