using LearningTracker.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Api.Handlers;

public class CategoryNotFoundHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public CategoryNotFoundHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not CategoryNotFoundException)
            return false;                       // не моe, пусть решает следующий

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails { Status = StatusCodes.Status404NotFound, Title = "Категория не найдена", Detail = exception.Message },
            options: null,
            contentType: "application/problem+json",
            cancellationToken: ct);

        return true;
    }
}
