using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Contexts;

namespace UrlShortener.Tests.Shared.Fixtures;

/// <summary>
/// Provides reusable database context creation methods for integration tests.
/// </summary>
public static class DbContextFixture
{
    /// <summary>
    /// Creates an in-memory EF Core database context using the InMemory provider.
    /// Use this for tests that don't require database constraint validation.
    /// </summary>
    public static ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    /// <summary>
    /// Creates an in-memory SQLite database context.
    /// Use this for tests that require database constraint validation (e.g., unique constraints).
    /// </summary>
    public static ApplicationDbContext CreateSqliteInMemoryContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}