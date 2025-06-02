using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class MongoMovieService : IMovieService
{
    private readonly IMongoCollection<Movie> _movieCollection;
    private const string mongoDbDatabaseName = "gbs";
    private const string mongoDbCollectionName = "movies";
    private readonly string _mongoDbConnectionString;
    public MongoMovieService(IOptions<DatabaseSettings> options)
    {
        _mongoDbConnectionString = options.Value.ConnectionString;

        var mongoClient = new MongoClient(_mongoDbConnectionString);
        var database = mongoClient.GetDatabase(mongoDbDatabaseName);
        _movieCollection = database.GetCollection<Movie>(mongoDbCollectionName); 


    }

    public string Check()
    {
        try
        {
            var client = new MongoClient(_mongoDbConnectionString);
            var databases = client.ListDatabaseNames().ToList();
            return "Access to MongoDB ok. Existing Databases: " + string.Join(", ", databases);
        }
        catch (System.Exception e)
        {
            return "Acces to MongoDB doesn't work: " + e.Message;
        }
    }

    public void Create(Movie movie)
    {
        _movieCollection.InsertOne(movie);
    }

    public IEnumerable<Movie> Get()
    {
        return _movieCollection.Find(m => true).ToList();
    }

    public Movie Get(string id)
    {
        return _movieCollection.Find(m => m.Id == id).FirstOrDefault();
    }

    public void Update(string id, Movie movie)
    {
        movie.Id = id;
        _movieCollection.ReplaceOne(m => m.Id == id, movie);
    }

    public void Remove(string id)
    {
        _movieCollection.DeleteOne(m => m.Id == id);
    }
}