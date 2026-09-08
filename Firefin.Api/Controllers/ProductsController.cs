using Firefin.Api.DTOs;
using Firefin.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Firefin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _products;

    public ProductsController(IProductService products) => _products = products;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll()
        => Ok(await _products.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _products.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ProductDto>> GetBySlug(string slug)
    {
        var product = await _products.GetBySlugAsync(slug);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
    {
        var created = await _products.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDto>> Update(int id, UpdateProductDto dto)
    {
        var updated = await _products.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => await _products.DeleteAsync(id) ? NoContent() : NotFound();
}
