using Firefin.Api.Data;
using Firefin.Api.DTOs;
using Firefin.Api.Mapping;
using Firefin.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Firefin.Api.Services;

public class ProductService : IProductService
{
    private readonly FirefinDbContext _db;

    public ProductService(FirefinDbContext db) => _db = db;

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        var products = await _db.Products
            .Include(p => p.Recipes)
            .OrderBy(p => p.Name)
            .ToListAsync();
        return products.Select(p => p.ToDto()).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _db.Products.Include(p => p.Recipes)
            .FirstOrDefaultAsync(p => p.Id == id);
        return product?.ToDto();
    }

    public async Task<ProductDto?> GetBySlugAsync(string slug)
    {
        var product = await _db.Products.Include(p => p.Recipes)
            .FirstOrDefaultAsync(p => p.Slug == slug);
        return product?.ToDto();
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Slug = await UniqueSlugAsync(Slugger.Slugify(dto.Name)),
            Type = dto.Type,
            Status = ProductStatus.Concept,
            HeatLevel = dto.HeatLevel,
            Description = dto.Description,
            TargetPrice = dto.TargetPrice
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product.ToDto();
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _db.Products.Include(p => p.Recipes)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product is null) return null;

        product.Name = dto.Name;
        product.Status = dto.Status;
        product.HeatLevel = dto.HeatLevel;
        product.Description = dto.Description;
        product.TargetPrice = dto.TargetPrice;
        product.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return product.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null) return false;

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }

    private async Task<string> UniqueSlugAsync(string baseSlug)
    {
        var slug = string.IsNullOrEmpty(baseSlug) ? "product" : baseSlug;
        var candidate = slug;
        var suffix = 2;
        while (await _db.Products.AnyAsync(p => p.Slug == candidate))
            candidate = $"{slug}-{suffix++}";
        return candidate;
    }
}
