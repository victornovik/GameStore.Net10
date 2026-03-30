# GameStore with ASP.NET Core 10

## Make sure .NET SDK 10 is installed
```powershell
dotnet --info
```

## Upgrade .NET version
```powershell
dotnet tool install -g upgrade-assistant
upgrade-assistant upgrade GameStore.Api.csproj --target-tfm net10.0
```

## Display all available project templates
```powershell
dotnet new list
```

## Create project, build and run
```powershell
dotnet new web -n GameStore.Api
dotnet build
dotnet dev-certs https --trust
dotnet run
dotnet run --launch-profile https
```

## Add EFCore with Sqlite provider
```powershell
dotnet tool install --global dotnet-ef
# Commands:
#  database    Commands to manage the database.
#  dbcontext   Commands to manage DbContext types.
#  migrations  Commands to manage migrations.

dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

## Create an initial migration and apply it to the database
```powershell
dotnet ef migrations add InitialCreate --output-dir Data\Migrations
dotnet ef database update
```

## Pass the connection string via an environment variable
```powershell
$env:ConnectionStrings__GameStore="Data Source=GameStore.PROD.db"

# Show all environment variables
Get-ChildItem Env:

# Run application from the same Powershell terminal
dotnet run
``` 

## Add standard MS circuit-breaker instead of Polly
```powershell
dotnet add package Microsoft.Extensions.Http.Resilience
```

##  Useful links
- [Resilience strategies](https://www.pollydocs.org/strategies/index.html)
- [Build resilient HTTP apps](https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience)
