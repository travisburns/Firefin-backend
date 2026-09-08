using Firefin.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Firefin.Api.Tests;

/// <summary>Builds an isolated in-memory FirefinDbContext per test.</summary>
internal static class TestDb
{
    public static FirefinDbContext Create()
    {
        var options = new DbContextOptionsBuilder<FirefinDbContext>()
            .UseInMemoryDatabase($"firefin-{Guid.NewGuid()}")
            .Options;
        return new FirefinDbContext(options);
    }
}
