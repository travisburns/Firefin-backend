using Firefin.Api.Data;
using Firefin.Api.DTOs;
using Firefin.Api.Mapping;
using Firefin.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Firefin.Api.Services;

public class OrderService : IOrderService
{
    private readonly FirefinDbContext _db;

    public OrderService(FirefinDbContext db) => _db = db;

    public async Task<ServiceResult<OrderDto>> CreateAsync(CreateOrderDto dto)
    {
        if (dto.Items is null || dto.Items.Count == 0)
            return ServiceResult<OrderDto>.Fail("An order must contain at least one item.");

        var order = new Order
        {
            OrderNumber = await NextOrderNumberAsync(),
            Status = OrderStatus.Pending,
            CustomerName = dto.CustomerName.Trim(),
            CustomerEmail = dto.CustomerEmail.Trim(),
            ShippingLine1 = dto.ShippingLine1.Trim(),
            ShippingLine2 = string.IsNullOrWhiteSpace(dto.ShippingLine2) ? null : dto.ShippingLine2.Trim(),
            ShippingCity = dto.ShippingCity.Trim(),
            ShippingRegion = string.IsNullOrWhiteSpace(dto.ShippingRegion) ? null : dto.ShippingRegion.Trim(),
            ShippingPostalCode = dto.ShippingPostalCode.Trim(),
            ShippingCountry = dto.ShippingCountry.Trim()
        };

        foreach (var line in dto.Items)
        {
            if (line.Quantity <= 0)
                return ServiceResult<OrderDto>.Fail($"Quantity must be positive for '{line.Title}'.");

            var built = await BuildItemAsync(line);
            if (!built.Ok) return ServiceResult<OrderDto>.Fail(built.Error!);
            order.Items.Add(built.Value!);
        }

        order.Subtotal = order.Items.Sum(i => i.LineTotal);

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return ServiceResult<OrderDto>.Success(order.ToDto());
    }

    public async Task<ServiceResult<OrderDto>> PayAsync(int orderId)
    {
        var order = await _db.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is null) return ServiceResult<OrderDto>.Fail("Order not found.");
        if (order.Status != OrderStatus.Pending)
            return ServiceResult<OrderDto>.Fail($"Order is {order.Status} and cannot be paid.");

        order.Status = OrderStatus.Paid;
        order.PaidAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return ServiceResult<OrderDto>.Success(order.ToDto());
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _db.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
        return order?.ToDto();
    }

    public async Task<OrderDto?> GetByNumberAsync(string orderNumber)
    {
        var order = await _db.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        return order?.ToDto();
    }

    public async Task<IReadOnlyList<OrderDto>> GetAllAsync()
    {
        var orders = await _db.Orders.Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
        return orders.Select(o => o.ToDto()).ToList();
    }

    /// <summary>
    /// Builds one order line. Catalog products are re-priced from the database
    /// (the client price is never trusted); standalone lines such as freezer
    /// boxes use the provided price, which must be positive.
    /// </summary>
    private async Task<ServiceResult<OrderItem>> BuildItemAsync(CreateOrderItemDto line)
    {
        int? productId = null;
        decimal unitPrice;

        if (!string.IsNullOrWhiteSpace(line.ProductSlug))
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Slug == line.ProductSlug);
            if (product is null)
                return ServiceResult<OrderItem>.Fail($"Unknown product '{line.ProductSlug}'.");
            if (product.Status != ProductStatus.Locked && product.Status != ProductStatus.Live)
                return ServiceResult<OrderItem>.Fail($"'{product.Name}' is not available for sale.");
            if (product.TargetPrice is null or <= 0)
                return ServiceResult<OrderItem>.Fail($"'{product.Name}' has no price.");

            productId = product.Id;
            unitPrice = product.TargetPrice.Value;
        }
        else
        {
            if (line.UnitPrice <= 0)
                return ServiceResult<OrderItem>.Fail($"'{line.Title}' has an invalid price.");
            unitPrice = line.UnitPrice;
        }

        return ServiceResult<OrderItem>.Success(new OrderItem
        {
            ProductId = productId,
            Title = line.Title.Trim(),
            Subtitle = string.IsNullOrWhiteSpace(line.Subtitle) ? null : line.Subtitle.Trim(),
            UnitPrice = unitPrice,
            Quantity = line.Quantity,
            LineTotal = unitPrice * line.Quantity
        });
    }

    private async Task<string> NextOrderNumberAsync()
    {
        for (var attempt = 0; attempt < 8; attempt++)
        {
            var candidate = $"FF-{DateTime.UtcNow:yyMMdd}-{RandomSuffix()}";
            if (!await _db.Orders.AnyAsync(o => o.OrderNumber == candidate))
                return candidate;
        }
        // Extremely unlikely fallback.
        return $"FF-{DateTime.UtcNow:yyMMddHHmmssfff}";
    }

    private static string RandomSuffix()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        return new string(Enumerable.Range(0, 4)
            .Select(_ => chars[Random.Shared.Next(chars.Length)])
            .ToArray());
    }
}
