using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FileManagementApp.Presentation.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly ProblemDetailsFactory problemDetailsFactory;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next, 
        IWebHostEnvironment webHostEnvironment, 
        ProblemDetailsFactory problemDetailsFactory,
        ILogger<ExceptionHandlingMiddleware> logger
        )
    {
        this.next = next;
        this.webHostEnvironment = webHostEnvironment;
        this.problemDetailsFactory = problemDetailsFactory;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var problem = problemDetailsFactory.CreateProblemDetails(context, 500, detail: webHostEnvironment.IsDevelopment() ? ex.ToString() : "Internal server error");
            context.Response.StatusCode = 500;

            logger.LogError(ex, "An unhandled exception occurred while processing the request.");

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
