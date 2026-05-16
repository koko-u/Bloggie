using System.Collections.Generic;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Bloggie.Web.Models.Forms;

public sealed class ImageValidator : AbstractValidator<IFormFile>
{
    private readonly HashSet<string> _permittedContentTypes =
    [
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/webp",
    ];

    private const long MaxBytes = 5 * 1024 * 1024;

    public ImageValidator()
    {
        RuleFor(image => image.Length)
            .NotEqual(0)
            .WithMessage("Image cannot be empty")
            .LessThanOrEqualTo(MaxBytes)
            .WithMessage("Image size exceeds the limit: {ComparisonValue}");

        RuleFor(image => image.ContentType)
            .Must(contentType => _permittedContentTypes.Contains(contentType))
            .WithMessage("Invalid image format");
    }
}
