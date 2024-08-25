using FooBooLooGameAPI.Data;
using FooBooLooGameAPI.Repositories.Implementations;
using FooBooLooGameAPI.Repositories.Interfaces;
using FooBooLooGameAPI.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Create a single instance of NpgsqlDataSourceBuilder
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
dataSourceBuilder.EnableDynamicJson(); // Enable dynamic JSON serialization

// Build the data source once and reuse it
var dataSource = dataSourceBuilder.Build();

// Add services to the container.
builder.Services.AddDbContext<GameDbContext>(options =>
{
    options.UseNpgsql(dataSource);
});

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<SessionService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Use the CORS policy
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.MapControllers();

app.Run();