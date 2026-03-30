using GameStore.Worker;
using Microsoft.Extensions.Http.Resilience;
using Polly;

var builder = Host.CreateApplicationBuilder(args);

//builder.Services.Configure<HostOptions>(hostOptions =>
//{
//    hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
//});

builder.Services.AddHttpClient<GamesClient>(client =>
{
    client.BaseAddress = new("http://localhost:5161");
})

// Add default settings of all delays and timeouts
//.AddStandardResilienceHandler()

.AddResilienceHandler("pipeline", pipelineBuilder =>
{
    // No more than 3 concurrent requests simultaneously
    pipelineBuilder.AddConcurrencyLimiter(3);

    pipelineBuilder.AddRetry(new HttpRetryStrategyOptions
    {
        BackoffType = DelayBackoffType.Exponential,
        MaxRetryAttempts = 5,
        Delay = TimeSpan.FromMilliseconds(100),
        UseJitter = false,
        OnRetry = static args =>
        {
            Console.WriteLine($"    Retry {args.AttemptNumber} after {args.RetryDelay.TotalMilliseconds:F2}ms, due to {args.Outcome.Result?.StatusCode.ToString() ?? args.Outcome.Exception?.GetType().Name}");
            return default;
        }
    });
    
    pipelineBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
    {
        SamplingDuration = TimeSpan.FromSeconds(5),
        FailureRatio = 0.9,
        MinimumThroughput = 5,
        BreakDuration = TimeSpan.FromSeconds(5),
    });

    pipelineBuilder.AddTimeout(TimeSpan.FromSeconds(1));
});

builder.Services.AddHostedService<Worker>(); 

var host = builder.Build();
host.Run();