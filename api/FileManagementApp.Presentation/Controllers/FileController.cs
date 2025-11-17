using FileManagementApp.Application.Features.Files.Commands.Delete;
using FileManagementApp.Application.Features.Files.Commands.Download;
using FileManagementApp.Application.Features.Files.Commands.GetAll;
using FileManagementApp.Application.Features.Files.Commands.Upload;
using FileManagementApp.Presentation.Common;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;

namespace FileManagementApp.Controllers;

/// <summary>
/// Controller for managing file operations such as upload, download, retrieval, and deletion.
/// </summary>
[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly ProblemDetailsFactory problemDetailsFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileController"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="problemDetailsFactory"></param>
    public FileController(IMediator mediator, ProblemDetailsFactory problemDetailsFactory)
    {
        this.mediator = mediator;
        this.problemDetailsFactory = problemDetailsFactory;
    }

    /// <summary>
    /// Uploads a file to the server. Muliple files can be uploaded in a single request.
    /// Multiple file uploads can partially succeed; in such cases, 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("upload")]
    [ProducesResponseType(201)]
    [MultipartFormData]
    [DisableFormValueModelBinding]
    [RequestSizeLimit(long.MaxValue)] // File size limit is handled in the application layer
    public async Task<IActionResult> UploadFile(CancellationToken cancellationToken)
    {
        MultipartSection? section;
        bool fileFound = false;
        
        var boundary = Request.GetMultipartBoundary();

        var reader = new MultipartReader(boundary, Request.Body);

        while ((section = await reader.ReadNextSectionAsync(cancellationToken)) != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection == null)
            {
                continue;
            }

            fileFound = true;

            if(fileSection.FileStream == null)
            {
                return problemDetailsFactory.BadRequest($"File '{fileSection.FileName}' has no content.", HttpContext);
            }

            if(section.ContentType == null) {
                return problemDetailsFactory.BadRequest($"File '{fileSection.FileName}' has no content type.", HttpContext);
            }

            var result = await mediator.Send(new UploadFileCommand(
                fileSection.FileName, 
                fileSection.FileStream,
                section.ContentType
                ), cancellationToken);

            if (!result.IsSuccess)
            {
                var problemDetails = result.Error!.ToProblemDetails(problemDetailsFactory, HttpContext);
                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }
        }

        if (!fileFound)
        {
            var problemDetails = problemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: StatusCodes.Status400BadRequest,
                detail: "No file found in the request."
            );
            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
        }

        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Retrieves a list of all uploaded files.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<FileResponse>), 200)]
    public async Task<IActionResult> GetFiles(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllFilesQuery(), cancellationToken);
        
        return result.ToActionResult(problemDetailsFactory, HttpContext, System.Net.HttpStatusCode.OK);
    }

    /// <summary>
    /// Downloads a file by name.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{fileName}")]
    [ProducesResponseType(typeof(FileStreamResult), 200)]
    public async Task<IActionResult> DownloadFile(string fileName, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DownloadFileQuery(fileName), cancellationToken);
        
        if (!result.IsSuccess)
        {
            var problemDetails = result.Error!.ToProblemDetails(problemDetailsFactory, HttpContext);

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
        }

        return File(result.Data!.FileStream, result.Data.ContentType);
    }

    /// <summary>
    /// Deletes a file by name.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{fileName}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteFile(string fileName, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteFileCommand(fileName), cancellationToken);
        
        return result.ToActionResult(problemDetailsFactory, HttpContext, System.Net.HttpStatusCode.NoContent);
    }
}
