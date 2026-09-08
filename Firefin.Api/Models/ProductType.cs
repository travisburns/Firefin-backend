namespace Firefin.Api.Models;

/// <summary>
/// The kind of product Firefin makes. Drives catalog grouping and, later,
/// which cooking/packaging rules apply.
/// </summary>
public enum ProductType
{
    Meal,
    Sauce,
    FireDrop
}
