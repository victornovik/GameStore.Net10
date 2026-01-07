using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    // Apply all migrations that have not been applied yet
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameStore");

        // DbContext has a Scoped service lifetime because:
        // 1. It ensures that a new instance of DbContext is created per HTTP request
        // 2. DB connections are a limited and expensive resource
        // 3. DbContext is thread-unsafe. Scoped avoids concurrency issues
        // 4. Makes it easier to manage transactions and ensure data consistency
        // 5. Reusing a DbContext instance can lead to increased memory usage as it tracks changes of entities over its lifetime
        //
        // builder.Services.AddSqlite actually calls builder.Services.AddScoped<GameStoreContext>

        builder.Services.AddSqlite<GameStoreContext>(
            connString,
            // Seeding lambda runs after EnsureCreated() is called or after migrations are applied.
            optionsAction: optionsBuilder => optionsBuilder.UseSeeding((context, _) =>
            {
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Name = "Fighting" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Platformer" },
                        new Genre { Name = "Racing" },
                        new Genre { Name = "Sports" }
                    );
                    context.SaveChanges();
                }
            })
        );
    }
}