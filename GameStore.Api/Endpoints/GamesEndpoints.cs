using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpoint = "GetGame";

    private static readonly List<GameDto> games = [
        new(1, "Street Fighter II", "Fighting", 19.99m, new DateOnly(1992,7,15)),
        new(2, "Final Fantasy VII", "RPG", 69.99m, new DateOnly(2024,2,29)),
        new(3, "Astro Bot", "Platformer", 59.99m, new DateOnly(2024,9,6))
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "dlrow olleh");

        var group = app.MapGroup("/games");

        group.MapGet("/", () => games);

        group.MapGet("/{id}", (int id) =>
        {
            var found = games.Find(g => g.Id == id);
            return found != null ? Results.Ok(found) : Results.NotFound();
        }).WithName(GetGameEndpoint);

        group.MapPost("/", (CreateGameDto g) =>
        {
            var newGame = new GameDto(games.Count + 1, g.Name, g.Genre, g.Price, g.ReleaseDate);
            games.Add(newGame);
            return Results.CreatedAtRoute(GetGameEndpoint, new { id = newGame.Id }, newGame);
        });

        group.MapPut("/{id}", (int id, UpdateGameDto changeling) =>
        {
            const int NotFound = -1;
            var index = games.FindIndex(g => g.Id == id);
            if (index == NotFound)
                return Results.NotFound();

            games[index] = new GameDto(id, changeling.Name, changeling.Genre, changeling.Price, changeling.ReleaseDate);
            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(g => g.Id == id);
            return Results.NoContent();
        });
    }
}