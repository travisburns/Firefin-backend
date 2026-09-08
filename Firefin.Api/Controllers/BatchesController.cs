using Firefin.Api.DTOs;
using Firefin.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Firefin.Api.Controllers;

[ApiController]
[Route("api")]
public class BatchesController : ControllerBase
{
    private readonly IBatchService _batches;

    public BatchesController(IBatchService batches) => _batches = batches;

    [HttpGet("recipes/{recipeId:int}/batches")]
    public async Task<ActionResult<IReadOnlyList<BatchDto>>> GetForRecipe(int recipeId)
    {
        var batches = await _batches.GetForRecipeAsync(recipeId);
        return batches is null ? NotFound() : Ok(batches);
    }

    [HttpGet("batches/{batchId:int}")]
    public async Task<ActionResult<BatchDto>> GetById(int batchId)
    {
        var batch = await _batches.GetByIdAsync(batchId);
        return batch is null ? NotFound() : Ok(batch);
    }

    [HttpPost("recipes/{recipeId:int}/batches")]
    public async Task<ActionResult<BatchDto>> Create(int recipeId, CreateBatchDto dto)
    {
        var created = await _batches.CreateAsync(recipeId, dto);
        return created is null
            ? NotFound()
            : CreatedAtAction(nameof(GetById), new { batchId = created.Id }, created);
    }
}
