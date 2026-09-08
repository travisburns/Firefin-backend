using Firefin.Api.DTOs;
using Firefin.Api.Models;
using Firefin.Api.Services;
using Xunit;

namespace Firefin.Api.Tests;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_generates_slug_and_starts_in_concept()
    {
        await using var db = TestDb.Create();
        var service = new ProductService(db);

        var created = await service.CreateAsync(
            new CreateProductDto("Blue Flame", ProductType.Sauce, 3, "Sweet-heat sauce", 12.99m));

        Assert.Equal("blue-flame", created.Slug);
        Assert.Equal(ProductStatus.Concept, created.Status);
    }

    [Fact]
    public async Task CreateAsync_makes_duplicate_names_unique()
    {
        await using var db = TestDb.Create();
        var service = new ProductService(db);

        var first = await service.CreateAsync(new CreateProductDto("Green Fire", ProductType.Meal, null, null, null));
        var second = await service.CreateAsync(new CreateProductDto("Green Fire", ProductType.Meal, null, null, null));

        Assert.Equal("green-fire", first.Slug);
        Assert.Equal("green-fire-2", second.Slug);
    }
}
