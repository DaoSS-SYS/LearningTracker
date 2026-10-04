using LearningTracker.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Api.Handlers;

public class TopicNotFoundExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public TopicNotFoundExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not TopicNotFoundException)
            return false;                       // не моe, пусть решает следующий

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails { Status = StatusCodes.Status404NotFound, Title = "Тема не найдена", Detail = exception.Message },
            options: null,
            contentType: "application/problem+json",
            cancellationToken: ct);
        
        return true;
    }
}
