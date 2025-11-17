using FileManagementApp.Application.Common.Dtos;
using FileManagementApp.Application.Common.Extensions;
using FluentValidation;

namespace FileManagementApp.Application.Features.Files.Commands.Delete;

public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
{
    public DeleteFileCommandValidator()
    {
        RuleFor(x => x.FileName)
            .ValidFileName();

        RuleFor(x => x.FileName)
            .ValidFileExtension();
    }
}
