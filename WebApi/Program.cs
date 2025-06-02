using MongoDB.Driver;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

var movieDatabaseConficSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(movieDatabaseConficSection);

builder.Services.AddSingleton<IMovieService, MongoMovieService>();
var app = builder.Build();

app.MapGet("/", () => "Minimal API Version 1.0");

app.MapGet("/check", (IMovieService movieService) => {

    return movieService.Check();
    
});

app.MapPost("/api/movies", (IMovieService movieService, Movie movie) =>
{
    movieService.Create(movie);
    return Results.Ok(movie);

});

app.MapGet("api/movies", (IMovieService movieService) =>
{
    var movies = movieService.Get();
    return Results.Ok(movies);

});

app.MapGet("api/movies/{id}", (IMovieService movieService, string id) =>
{
    var movie = movieService.Get(id);
    return movie != null ? Results.Ok(movie) : Results.NotFound();
});

app.MapPut("/api/movies/{id}", (IMovieService movieService, string id, Movie movie) =>
{
    var existingMovie = movieService.Get(id);
    if (existingMovie == null)
    {
        return Results.NotFound();
    }

    movieService.Update(id, movie);
    return Results.Ok(movie);
});

app.MapDelete("api/movies/{id}", (IMovieService movieService, string id) =>
{
    var movie = movieService.Get(id);
    if (movie == null)
    {
        return Results.NotFound();
    }
    movieService.Remove(id);
    return Results.Ok();
});



app.Run();
