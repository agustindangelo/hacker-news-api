using System.Data;
using HackerNewsAPI.Managers.Contracts;
using HackerNewsAPI.Managers.Implementations;
using HackerNewsAPI.Repositories.Contracts;
using HackerNewsAPI.Repositories.Implementations;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new() 
    { 
        Title = "Hacker News API",
        Version = "v1" 
    }));
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddAutoMapper(typeof(Program)); 
builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://lively-grass-0bf2ced0f.6.azurestaticapps.net")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Configure SQLite in-memory database
var connection = new SqliteConnection("Data Source=:memory:");
connection.Open(); // Keep the connection open for the lifetime of the app
builder.Services.AddSingleton<IDbConnection>(connection);

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

// Register Managers
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IItemManager, ItemManager>();

var app = builder.Build();

// Seed the database at startup
SeedDatabase(connection);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hacker News API V1");
        options.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowWebClient");
app.MapControllers();
app.Run();

void SeedDatabase(SqliteConnection connection)
{
    var sqlFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData.sql");
    var sqlCommands = File.ReadAllText(sqlFilePath);

    using var command = connection.CreateCommand();
    command.CommandText = sqlCommands;
    command.ExecuteNonQuery();
}