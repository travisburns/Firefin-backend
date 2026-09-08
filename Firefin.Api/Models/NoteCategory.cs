namespace Firefin.Api.Models;

/// <summary>
/// Issue-log categories for batch observations, taken from the Pre-Stage 0
/// product-mastery checklist. Keeps R&amp;D notes structured so patterns are
/// searchable across batches rather than buried in free text.
/// </summary>
public enum NoteCategory
{
    Moisture,
    Heat,
    Adhesion,
    CheeseMelt,
    Separation,
    Cook,
    Texture,
    Packaging,
    Flavor,
    Other
}
