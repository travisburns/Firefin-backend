using Firefin.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Firefin.Api.Data;

/// <summary>
/// Seeds the first real R&amp;D subject: Blue Flame, the sauce currently being
/// developed. Kept idempotent so it is safe to run on every startup.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(FirefinDbContext db)
    {
        if (await db.Products.AnyAsync())
            return;

        var blueFlame = new Product
        {
            Slug = "blue-flame",
            Name = "Blue Flame",
            Type = ProductType.Sauce,
            Status = ProductStatus.InDevelopment,
            HeatLevel = 3,
            Description = "Signature sweet-heat sauce: blueberry + Fresno (or another bright red pepper) + garlic. First sauce in active development.",
            Recipes =
            {
                new Recipe
                {
                    Version = 1,
                    IsCurrent = true,
                    Notes = "Starting point. Gram weights are placeholders to be replaced by tested amounts.",
                    Ingredients =
                    {
                        new RecipeIngredient { Name = "Blueberry", Grams = 200m, SortOrder = 1 },
                        new RecipeIngredient { Name = "Fresno pepper", Grams = 60m, SortOrder = 2 },
                        new RecipeIngredient { Name = "Garlic", Grams = 15m, SortOrder = 3 },
                        new RecipeIngredient { Name = "Vinegar", Grams = 80m, SortOrder = 4 },
                        new RecipeIngredient { Name = "Sugar", Grams = 40m, SortOrder = 5 },
                        new RecipeIngredient { Name = "Salt", Grams = 6m, SortOrder = 6 }
                    },
                    Batches =
                    {
                        new Batch
                        {
                            BatchNumber = 1,
                            MadeOn = DateOnly.FromDateTime(DateTime.UtcNow),
                            Verdict = "First attempt — baseline",
                            Summary = "Baseline batch to anchor the iteration log.",
                            Notes =
                            {
                                new BatchNote
                                {
                                    Category = NoteCategory.Heat,
                                    WhatDidnt = "Record heat level vs blueberry sweetness balance here.",
                                    Severity = 1
                                }
                            }
                        }
                    }
                }
            }
        };

        db.Products.Add(blueFlame);
        await db.SaveChangesAsync();
    }
}
