using GameStore.Api.Dtos;

const string GetGameEndpointName = "GetGame";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    new(1, "Street Fighter II", "Fighting", 19.99m, new DateOnly(1992,7,15)),
    new(2, "Final Fantasy VII", "RPG", 69.99m, new DateOnly(2024,2,29)),
    new(3, "Astro Bot", "Platformer", 59.99m, new DateOnly(2024,9,6))
];

app.MapGet("/", () => "Hello World!");
app.MapGet("/games", () => games);
app.MapGet("/games/{id}", (int id) => games.Find(g => g.Id == id)).WithName(GetGameEndpointName);
app.MapPost("/games", (CreateGameDto g) =>
{
    var newGame = new GameDto(games.Count + 1, g.Name, g.Genre, g.Price, g.ReleaseDate);
    games.Add(newGame);
    return Results.CreatedAtRoute(GetGameEndpointName, new {id = newGame.Id}, newGame);
});
app.MapPut("games/{id}", (int id, UpdateGameDto changeling) =>
{
    var index = games.FindIndex(g => g.Id == id);
    games[index] = new GameDto(id, changeling.Name, changeling.Genre, changeling.Price, changeling.ReleaseDate);
    return Results.NoContent();
});
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(g => g.Id == id);
    return Results.NoContent();
});

app.Run();