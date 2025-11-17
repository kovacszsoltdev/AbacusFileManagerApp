using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace FileManagementApp.Presentation.Common;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MultipartFormDataAttribute: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.HasFormContentType ||
            !MediaTypeHeaderValue.TryParse(context.HttpContext.Request.ContentType, out var mediaTypeHeader) ||
            string.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))
        {
            context.Result = new UnsupportedMediaTypeResult();
        }
    }
}
