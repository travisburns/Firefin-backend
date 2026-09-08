using System.Text;
using System.Text.RegularExpressions;

namespace Firefin.Api.Services;

/// <summary>Turns a product name into a URL-friendly slug, e.g. "Blue Flame" -> "blue-flame".</summary>
public static partial class Slugger
{
    public static string Slugify(string input)
    {
        var lowered = input.Trim().ToLowerInvariant();
        var cleaned = NonSlugChars().Replace(lowered, "-");
        return cleaned.Trim('-');
    }

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NonSlugChars();
}
