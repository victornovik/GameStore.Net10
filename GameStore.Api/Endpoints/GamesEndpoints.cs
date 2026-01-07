using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    public static void MapGamesEndpoints(this WebApplication app)
    {
        app.MapGet("/", async () => await Task.FromResult("dlrow olleh"));

        var group = app.MapGroup("/games");

        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Games
            .Include(game => game.Genre)
            .Select(game => new GameSummaryDto(game.Id, game.Name, game.Genre!.Name, game.Price, game.ReleaseDate))
            .AsNoTracking()
            .ToListAsync()
        );

        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(new GameDetailsDto(game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate)
            );
        }).WithName(GetGameEndpointName);

        group.MapPost("/", async (CreateGameDto g, GameStoreContext dbContext) =>
        {
            Game game = new () { Name=g.Name, GenreId=g.GenreId, Price=g.Price, ReleaseDate = g.ReleaseDate };
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(
                GetGameEndpointName, 
                routeValues: new { id = game.Id }, 
                value: new GameDetailsDto(game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate));
        });

        group.MapPut("/{id}", async (int id, UpdateGameDto changeling, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            if (game is null)
                return Results.NotFound();

            game.Name = changeling.Name;
            game.GenreId = changeling.GenreId;
            game.Price = changeling.Price;
            game.ReleaseDate = changeling.ReleaseDate;

            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }
}