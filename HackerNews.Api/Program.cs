using System.Data;
using HackerNews.Api.Managers.Contracts;
using HackerNews.Api.Managers.Implementations;
using HackerNews.Api.Repositories.Contracts;
using HackerNews.Api.Repositories.Implementations;
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
connection.Open();
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
    app.UseSwaggerUI();
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