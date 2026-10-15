using LearningTracker.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace LearningTracker.Api.Handlers;

public class InvalidCredentialsHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public InvalidCredentialsHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not InvalidCredentialsException)
            return false;                       // не моe, пусть решает следующий

        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails { Status = StatusCodes.Status401Unauthorized, Title = "Не авторизован", Detail = exception.Message },
            options: null,
            contentType: "application/problem+json",
            cancellationToken: ct);

        return true;
    }
}
