namespace GameStore.Worker;

public class Worker(GamesClient client) : BackgroundService
{
    public override Task StartAsync(CancellationToken ct)
    {
        Console.WriteLine($"Sending request to {client.ApiAddress} ...");
        return base.StartAsync(ct);
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            //Console.WriteLine("GetGamesAsync() ...");
            await client.GetGamesAsync();
            await Task.Delay(1000, ct); 
        }
    }
}