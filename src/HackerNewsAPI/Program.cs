using System.Data;
using HackerNewsAPI.Managers.Contracts;
using HackerNewsAPI.Managers.Implementations;
using HackerNewsAPI.Repositories.Contracts;
using HackerNewsAPI.Repositories.Implementations;
using Microsoft.Data.SqlClient;

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
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

// Register Managers
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IItemManager, ItemManager>();

var app = builder.Build();

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