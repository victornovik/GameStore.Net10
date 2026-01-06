# GameStore with ASP.NET Core 10

## Make sure .NET SDK 10 is installed
```powershell
dotnet --info
```

## Display all available project templates
```powershell
dotnet new list
```

## Create project, build, run
```powershell
dotnet new web -n GameStore.Api

dotnet dev-certs https --trust
dotnet build
dotnet run --launch-profile https
```

## EF dependencies
```powershell
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

## Create an initial migration
```powershell
dotnet ef migrations add initial
dotnet ef migrations add bids
```

## Apply the initial migration to the database
```powershell
dotnet ef database update
```