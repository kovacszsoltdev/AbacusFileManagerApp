using FileManagementApp.Application.Common.Dtos;
using FileManagementApp.Application.Common.Extensions;
using FluentValidation;

namespace FileManagementApp.Application.Features.Files.Commands.Download;

public class DownloadFileQueryValidator : AbstractValidator<DownloadFileQuery>
{
    public DownloadFileQueryValidator()
    {
        RuleFor(x => x.FileName)
            .ValidFileName();

        RuleFor(x => x.FileName)
            .ValidFileExtension();
    }
}
