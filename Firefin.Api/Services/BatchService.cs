using Firefin.Api.Data;
using Firefin.Api.DTOs;
using Firefin.Api.Mapping;
using Firefin.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Firefin.Api.Services;

public class BatchService : IBatchService
{
    private readonly FirefinDbContext _db;

    public BatchService(FirefinDbContext db) => _db = db;

    public async Task<IReadOnlyList<BatchDto>?> GetForRecipeAsync(int recipeId)
    {
        if (!await _db.Recipes.AnyAsync(r => r.Id == recipeId))
            return null;

        var batches = await _db.Batches
            .Where(b => b.RecipeId == recipeId)
            .Include(b => b.Notes)
            .OrderByDescending(b => b.BatchNumber)
            .ToListAsync();

        return batches.Select(b => b.ToDto()).ToList();
    }

    public async Task<BatchDto?> GetByIdAsync(int batchId)
    {
        var batch = await _db.Batches
            .Include(b => b.Notes)
            .FirstOrDefaultAsync(b => b.Id == batchId);
        return batch?.ToDto();
    }

    public async Task<BatchDto?> CreateAsync(int recipeId, CreateBatchDto dto)
    {
        if (!await _db.Recipes.AnyAsync(r => r.Id == recipeId))
            return null;

        var nextNumber = await _db.Batches
            .Where(b => b.RecipeId == recipeId)
            .Select(b => (int?)b.BatchNumber)
            .MaxAsync() ?? 0;
        nextNumber++;

        var batch = new Batch
        {
            RecipeId = recipeId,
            BatchNumber = nextNumber,
            MadeOn = dto.MadeOn,
            MadeBy = dto.MadeBy,
            Rating = dto.Rating,
            Verdict = dto.Verdict,
            Summary = dto.Summary,
            Notes = (dto.Notes ?? new List<CreateBatchNoteDto>()).Select(n => new BatchNote
            {
                Category = n.Category,
                WhatWorked = n.WhatWorked,
                WhatDidnt = n.WhatDidnt,
                Severity = n.Severity
            }).ToList()
        };

        _db.Batches.Add(batch);
        await _db.SaveChangesAsync();

        return batch.ToDto();
    }
}
