using Firefin.Api.DTOs;

namespace Firefin.Api.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto?> GetBySlugAsync(string slug);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
}
