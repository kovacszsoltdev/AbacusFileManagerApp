using FileManagementApp.Application.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace FileManagementApp.Presentation.Common;

public static class ActionResultMapper
{
    public static ProblemDetails ToProblemDetails(
        this ApplicationError error, 
        ProblemDetailsFactory factory, 
        HttpContext httpContext)
    {
        var status = MapStatus(error.Code);
        
        var problemDetails = factory.CreateProblemDetails(
            httpContext,
            statusCode: status,
            detail: error.Message
        );

        problemDetails.Extensions["errorCode"] = error.Code;

        if (error.Details is not null)
        {
            problemDetails.Extensions["details"] = error.Details;
        }

        return problemDetails;
    }

    public static IActionResult ToActionResult<T>(
        this ApplicationResult<T> result,
        ProblemDetailsFactory factory,
        HttpContext httpContext,
        HttpStatusCode successStatusCode)
    {
        if(result.IsSuccess)
        {
            return new ObjectResult(result.Data)
            {
                StatusCode = (int)successStatusCode
            };
        }
        
        var problemDetails = result.Error!.ToProblemDetails(factory, httpContext);
        
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    public static IActionResult ToActionResult(
        this ApplicationResult result,
        ProblemDetailsFactory factory,
        HttpContext httpContext,
        HttpStatusCode successStatusCode)
    {
        if(result.IsSuccess)
        {
            return new StatusCodeResult((int)successStatusCode);
        }
        
        var problemDetails = result.Error!.ToProblemDetails(factory, httpContext);
        
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    public static IActionResult BadRequest(this ProblemDetailsFactory problemDetailsFactory, string message, HttpContext httpContext)
    {
        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            httpContext,
            statusCode: StatusCodes.Status400BadRequest,
            detail: message
        );
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    private static int MapStatus(string appErrorCode)
    {
        return appErrorCode switch
        {
            ErrorCodes.UNEXPECTED => StatusCodes.Status500InternalServerError,
            ErrorCodes.VALIDATION_ERROR => StatusCodes.Status400BadRequest,
            ErrorCodes.FileService.FILE_NOT_FOUND => StatusCodes.Status404NotFound,
            ErrorCodes.FileService.FILE_TOO_LARGE => StatusCodes.Status413PayloadTooLarge,
            ErrorCodes.FileService.FILE_ALREADY_EXISTS => StatusCodes.Status409Conflict,
            ErrorCodes.FileService.UNAUTHORIZED_FILE_ACCESS => StatusCodes.Status403Forbidden,
            ErrorCodes.FileService.FILE_SERVICE_NOT_AVAILABLE => StatusCodes.Status503ServiceUnavailable,                        
            _ => StatusCodes.Status500InternalServerError
        };
    }
}