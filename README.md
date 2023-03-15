# Imdb Console
> [High school](https://kyberna.cz/) assignment 2st year (2019)

## Minimum requirements
- [.NET Core 3.0](https://dotnet.microsoft.com/en-us/download/dotnet/3.0)
- [MSSQL 2019](https://www.microsoft.com/en-us/sql-server/sql-server-2019)

## Required nuggets
- Microsoft.AspNet.WebApi.Client @ 5.2.7
- Microsoft.EntityFrameworkCore.SqlServer @ 5.0.5
- Microsoft.EntityFrameworkCore.Tools @ 5.0.5

## Before start
Default db [connection string](https://github.com/kaldaf/ImdbConsole/blob/main/ImdbConsole/Program.cs#L1111): `Data source=(localdb)\\MSSQLLocalDB;Integrated security=True;Initial Catalog=ImdbHomework`.

```shell
dotnet ef database update
```

## What the application includes
- Movies
  - Add movie
  - Add movie from OMDb
  - List of movies
  - Movie Information
  - Open link
  - Add genre
  - Add another title to the film
  - Managing Film
  - Change movie title
  - Change movie description
  - Change the rating of a movie
  - Change movie link
  - Delete movie
  
- Actors
  - Add an actor
  - List of actors
  - Assign actors to a film
  - Remove actors from a movie
  - Delete Actor
  - List all actors
