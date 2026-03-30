namespace GameStore.Worker;

public record GameDto(int Id, string Name, string Genre, decimal Price, DateOnly ReleaseDate);

public class GamesClient(HttpClient httpClient)
{
    public async Task<IReadOnlyCollection<GameDto>> GetGamesAsync()
    {
        var items = await httpClient.GetFromJsonAsync<IReadOnlyCollection<GameDto>>("/games");
        return items;
    }

    public string ApiAddress => httpClient.BaseAddress.ToString();
}