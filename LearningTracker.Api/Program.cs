using LearningTracker.Api.Data;
using LearningTracker.Api.Handlers;
using LearningTracker.Api.Repositories;
using LearningTracker.Api.Repositories.Interfaces;
using LearningTracker.Api.Services;
using LearningTracker.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Writing logs to console and file(everyday)
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
        .MinimumLevel
        .Override("Microsoft.AspNetCore", LogEventLevel.Warning);
});
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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register handler to notfound and topicHasSessions
builder.Services.AddExceptionHandler<InvalidCredentialsHandler>();
builder.Services.AddExceptionHandler<TopicNotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<TopicHasSessionsExceptionHandler>();
builder.Services.AddExceptionHandler<UserAlreadyExistsHandler>();
builder.Services.AddExceptionHandler<CategoryHasTopicsHandler>();
builder.Services.AddExceptionHandler<CategoryNotFoundHandler>();
builder.Services.AddExceptionHandler<CategoryNameIsBusyHandler>();
// To global own answer to problems
builder.Services.AddProblemDetails();

/*var configuration = new ConfigurationBuilder() // чтобы брать значения из appsettings
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();*/

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // для подключения JwtToken
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Настройка Jwt:Key не задана"))),
        };
    });

var app = builder.Build();

// To catch global problems
app.UseExceptionHandler(); // работает как обертка для всего, что ниже. Все ошибки которые будут внизу - пролетят выше через него и он их поймает.

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

app.UseAuthentication(); // для проверки токена

app.UseAuthorization();

app.MapControllers();

app.Run();
// все что после builder.build - называется Middleware, дальше контейнеры вкладыванием друг в друга идут по цепочке в низ, и так же возвращают ответ