using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        const string GetGenreEndpointName = "GetGenre";

        var group = app.MapGroup("/genres");

        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Genres
            .Select(g => new GenreDto(g.Id, g.Name))
            .AsNoTracking()
            .ToListAsync()
        );

        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var g = await dbContext.Genres.FindAsync(id);
            return g is null ? Results.NotFound() : Results.Ok(new GenreDto(g.Id, g.Name));
        }).WithName(GetGenreEndpointName);

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Genres
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }
}