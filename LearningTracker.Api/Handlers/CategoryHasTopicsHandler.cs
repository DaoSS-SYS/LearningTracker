using LearningTracker.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LearningTracker.Api.Handlers;

public class CategoryHasTopicsHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public CategoryHasTopicsHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not CategoryHasTopicsException)
            return false;                       // не моe, пусть решает следующий

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails { Status = StatusCodes.Status409Conflict, Title = "Категория не может быть удалена, она содержит темы", Detail = exception.Message },
            options: null,
            contentType: "application/problem+json",
            cancellationToken: ct);

        return true;
    }
}
