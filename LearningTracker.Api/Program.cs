using LearningTracker.Api.Data;
using Microsoft.EntityFrameworkCore;
using LearningTracker.Api.Repositories;
using LearningTracker.Api.Repositories.Interfaces;
using LearningTracker.Api.Services;
using LearningTracker.Api.Services.Interfaces;
using LearningTracker.Api.Handlers;

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
builder.Services.AddScoped<IStudySessionRepository, StudySessionRepository>();
builder.Services.AddScoped<IStudySessionService, StudySessionService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register handler to notfound
builder.Services.AddExceptionHandler<TopicNotFoundExceptionHandler>();
// To global own answer to problems
builder.Services.AddProblemDetails();

var app = builder.Build();

// To catch global problems
app.UseExceptionHandler();

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
// все что после builder.build - называется Middleware, дальше контейнеры вкладыванием друг в друга идут по цепочке в низ, и так же возвращают ответ