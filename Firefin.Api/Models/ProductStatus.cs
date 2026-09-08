namespace Firefin.Api.Models;

/// <summary>
/// Lifecycle of a product from first idea to sellable SKU.
/// A product only becomes eligible for the customer catalog once it is Locked.
/// </summary>
public enum ProductStatus
{
    Concept,
    InDevelopment,
    Locked,
    Live,
    Retired
}
