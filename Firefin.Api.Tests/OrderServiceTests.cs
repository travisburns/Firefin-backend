using Firefin.Api.Data;
using Firefin.Api.DTOs;
using Firefin.Api.Models;
using Firefin.Api.Services;
using Xunit;

namespace Firefin.Api.Tests;

public class OrderServiceTests
{
    private static async Task<Product> SeedSellableAsync(FirefinDbContext db, decimal price)
    {
        var product = new Product
        {
            Slug = "green-fire",
            Name = "Green Fire",
            Type = ProductType.Meal,
            Status = ProductStatus.Locked,
            TargetPrice = price
        };
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return product;
    }

    [Fact]
    public async Task CreateAsync_reprices_product_lines_from_catalog()
    {
        await using var db = TestDb.Create();
        await SeedSellableAsync(db, 14.99m);
        var service = new OrderService(db);

        // Client sends a bogus $0.01 price; server must ignore it.
        var result = await service.CreateAsync(new CreateOrderDto(
            "Travis", "travis@example.com", "1 Main", null, "Town", "ST", "00000", "USA",
            new[] { new CreateOrderItemDto("green-fire", "Green Fire", "Meal", 0.01m, 2) }));

        Assert.True(result.Ok);
        var order = result.Value!;
        Assert.Equal(14.99m, order.Items[0].UnitPrice);
        Assert.Equal(29.98m, order.Subtotal);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.StartsWith("FF-", order.OrderNumber);
    }

    [Fact]
    public async Task CreateAsync_rejects_unsellable_product()
    {
        await using var db = TestDb.Create();
        db.Products.Add(new Product
        {
            Slug = "blue-flame",
            Name = "Blue Flame",
            Type = ProductType.Sauce,
            Status = ProductStatus.InDevelopment,
            TargetPrice = 12.99m
        });
        await db.SaveChangesAsync();
        var service = new OrderService(db);

        var result = await service.CreateAsync(new CreateOrderDto(
            "Travis", "travis@example.com", "1 Main", null, "Town", "ST", "00000", "USA",
            new[] { new CreateOrderItemDto("blue-flame", "Blue Flame", "Sauce", 12.99m, 1) }));

        Assert.False(result.Ok);
        Assert.Contains("not available", result.Error);
    }

    [Fact]
    public async Task CreateAsync_trusts_price_for_standalone_box_line()
    {
        await using var db = TestDb.Create();
        var service = new OrderService(db);

        var result = await service.CreateAsync(new CreateOrderDto(
            "Travis", "travis@example.com", "1 Main", null, "Town", "ST", "00000", "USA",
            new[] { new CreateOrderItemDto(null, "Build Your Freezer — 6 meals", "6x Green Fire", 79.99m, 1) }));

        Assert.True(result.Ok);
        Assert.Equal(79.99m, result.Value!.Subtotal);
        Assert.Null(result.Value.Items[0].ProductId);
    }

    [Fact]
    public async Task CreateAsync_rejects_empty_order()
    {
        await using var db = TestDb.Create();
        var service = new OrderService(db);

        var result = await service.CreateAsync(new CreateOrderDto(
            "Travis", "travis@example.com", "1 Main", null, "Town", "ST", "00000", "USA",
            Array.Empty<CreateOrderItemDto>()));

        Assert.False(result.Ok);
    }

    [Fact]
    public async Task PayAsync_moves_pending_to_paid_and_blocks_double_pay()
    {
        await using var db = TestDb.Create();
        await SeedSellableAsync(db, 14.99m);
        var service = new OrderService(db);

        var created = await service.CreateAsync(new CreateOrderDto(
            "Travis", "travis@example.com", "1 Main", null, "Town", "ST", "00000", "USA",
            new[] { new CreateOrderItemDto("green-fire", "Green Fire", "Meal", 14.99m, 1) }));

        var paid = await service.PayAsync(created.Value!.Id);
        Assert.True(paid.Ok);
        Assert.Equal(OrderStatus.Paid, paid.Value!.Status);
        Assert.NotNull(paid.Value.PaidAt);

        var again = await service.PayAsync(created.Value.Id);
        Assert.False(again.Ok);
    }
}
