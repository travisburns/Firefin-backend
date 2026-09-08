using Firefin.Api.DTOs;
using Firefin.Api.Models;

namespace Firefin.Api.Mapping;

/// <summary>
/// Small, explicit entity-to-DTO projections. Kept as plain extension methods
/// (no mapper library) so the shape of every API response is obvious.
/// </summary>
public static class DtoMapping
{
    public static ProductDto ToDto(this Product p) => new(
        p.Id, p.Slug, p.Name, p.Type, p.Status, p.HeatLevel, p.Description,
        p.TargetPrice, p.Recipes.Count, p.CreatedAt, p.UpdatedAt);

    public static RecipeIngredientDto ToDto(this RecipeIngredient i) =>
        new(i.Id, i.Name, i.Grams, i.Notes, i.SortOrder);

    public static RecipeDto ToDto(this Recipe r) => new(
        r.Id, r.ProductId, r.Version, r.IsCurrent, r.Notes, r.CreatedAt,
        r.Ingredients.OrderBy(i => i.SortOrder).Select(i => i.ToDto()).ToList(),
        r.Batches.Count);

    public static BatchNoteDto ToDto(this BatchNote n) =>
        new(n.Id, n.Category, n.WhatWorked, n.WhatDidnt, n.Severity);

    public static BatchDto ToDto(this Batch b) => new(
        b.Id, b.RecipeId, b.BatchNumber, b.MadeOn, b.MadeBy, b.Rating,
        b.Verdict, b.Summary, b.CreatedAt,
        b.Notes.Select(n => n.ToDto()).ToList());
}
