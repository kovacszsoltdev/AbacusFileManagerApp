
using FileManagementApp.Application.Common.Dtos;
using FluentValidation;

namespace FileManagementApp.Application.Common.Extensions;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> ValidFileName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Matches("^[A-Za-z0-9._-]+$")
            .WithMessage("File name contains invalid characters. Allowed: letters, numbers, dot, dash, underscore.")
            .WithErrorCode(ErrorCodes.Validation.INVALID_FILE_NAME);
    }

    public static IRuleBuilderOptions<T, string> ValidFileExtension<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(fileName =>
            {
                var allowed = new[] { ".png", ".jpg", ".jpeg", ".pdf", ".txt" };
                return allowed.Any(ext => ext == Path.GetExtension(fileName));
            })
            .WithMessage("File extension not allowed.")
            .WithErrorCode(ErrorCodes.Validation.INVALID_FILE_EXTENSION);
    }

    public static IRuleBuilderOptions<T, string> ValidContentType<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(x =>
            {
                var allowedTypes = new[] { "image/png", "image/jpg", "image/jpeg", "application/pdf", "text/plain" };
                return allowedTypes.Contains(x);
            })
            .WithMessage("Content type not allowed")
            .WithErrorCode(ErrorCodes.Validation.INVALID_CONTENT_TYPE);
    }
}
