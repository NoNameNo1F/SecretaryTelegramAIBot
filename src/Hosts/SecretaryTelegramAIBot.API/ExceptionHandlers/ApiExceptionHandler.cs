﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SecretaryTelegramAIBot.API.ExceptionHandlers;

public class ApiExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public ApiExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = new ProblemDetails();
        
        switch (exception)
        {
            // case ValidationException validationException:
            //     problemDetails = new ProblemDetails
            //     {
            //         Title = "Validation Error",
            //         Detail = string.Join(", ", validationException.Errors.Select(e => e.ErrorMessage)),
            //         Status = StatusCodes.Status400BadRequest,
            //         Type = nameof(ValidationException),
            //         Extensions = { { "errors", validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) } }
            //     };
            //     break;
            
            // case CartNotFoundException:
            //     problemDetails = new ProblemDetails
            //     {
            //         Title = "User Cart was Not Found",
            //         Detail = exception.Message,
            //         Status = StatusCodes.Status404NotFound,
            //         Type = nameof(CartNotFoundException)
            //     };
            //     break;
            
            default:
                problemDetails = new ProblemDetails
                {
                    Title = "An error occurred",
                    Detail = exception.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Type = exception.GetType().Name
                };
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
    
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });
    }
}