using LearningTracker.Api.Data;
using Microsoft.EntityFrameworkCore;
using LearningTracker.Api.Repositories;
using LearningTracker.Api.Repositories.Interfaces;
using LearningTracker.Api.Services;
using LearningTracker.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Get connection lines
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Строка подключения DefaultConnection не найдена");

// Register contextDb
builder.Services.AddDbContext<LearningTrackerDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register DI 
builder.Services.AddScoped<ILearningTopicRepository, LearningTopicRepository>();
builder.Services.AddScoped<ILearningTopicService, LearningTopicService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "LearningTracker API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
