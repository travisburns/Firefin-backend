using System.ComponentModel.DataAnnotations;

namespace Firefin.Api.Models;

/// <summary>
/// A single structured observation on a batch: what worked, what did not, under
/// a category, with a severity so recurring problems are easy to spot.
/// </summary>
public class BatchNote
{
    public int Id { get; set; }

    public int BatchId { get; set; }
    public Batch? Batch { get; set; }

    public NoteCategory Category { get; set; }

    [MaxLength(1000)]
    public string? WhatWorked { get; set; }

    [MaxLength(1000)]
    public string? WhatDidnt { get; set; }

    /// <summary>1 = minor, 5 = blocking. Lets the Lab rank issues to fix next.</summary>
    public int Severity { get; set; } = 1;
}
