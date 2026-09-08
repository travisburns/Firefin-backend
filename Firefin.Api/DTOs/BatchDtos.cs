using Firefin.Api.Models;

namespace Firefin.Api.DTOs;

public record BatchNoteDto(
    int Id,
    NoteCategory Category,
    string? WhatWorked,
    string? WhatDidnt,
    int Severity);

public record BatchDto(
    int Id,
    int RecipeId,
    int BatchNumber,
    DateOnly MadeOn,
    string? MadeBy,
    int? Rating,
    string? Verdict,
    string? Summary,
    DateTime CreatedAt,
    IReadOnlyList<BatchNoteDto> Notes);

public record CreateBatchNoteDto(
    NoteCategory Category,
    string? WhatWorked,
    string? WhatDidnt,
    int Severity);

/// <summary>
/// Create a batch against a recipe. The service assigns the next batch number.
/// </summary>
public record CreateBatchDto(
    DateOnly MadeOn,
    string? MadeBy,
    int? Rating,
    string? Verdict,
    string? Summary,
    IReadOnlyList<CreateBatchNoteDto>? Notes);
