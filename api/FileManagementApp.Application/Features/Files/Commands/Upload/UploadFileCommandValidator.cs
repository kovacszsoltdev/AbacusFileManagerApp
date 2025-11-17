using FileManagementApp.Application.Common.Dtos;
using FileManagementApp.Application.Common.Extensions;
using FluentValidation;

namespace FileManagementApp.Application.Features.Files.Commands.Upload;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.FileName)
            .ValidFileName();

        RuleFor(x => x.FileName)
            .ValidFileExtension();

        RuleFor(x => x.ContentType)
            .ValidContentType();

        RuleFor(x => x.Stream)
            .NotNull()
            .Must(stream => stream.CanRead)
            .WithErrorCode(ErrorCodes.Validation.FILE_STREAM_NOT_READABLE);
    }
}
