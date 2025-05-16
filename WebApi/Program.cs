using MongoDB.Driver;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

var movieDatabaseConficSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(movieDatabaseConficSection);

var app = builder.Build();

app.MapGet("/", () => "Minimal API Version 1.0");

app.MapGet("/check", (Microsoft.Extensions.Options.IOptions<DatabaseSettings> options) => {

    try {
        var connectionString = options.Value.ConnectionString;
        var client = new MongoClient(connectionString);
        var databases = client.ListDatabaseNames().ToList();
        return "Access to MongoDB ok. Existing Databases: " + string.Join(", ", databases);
    }
    catch (System.Exception e)
    {
        return "Acces to MongoDB doesn't work: " + e.Message;
    }
    
});

app.Run();
