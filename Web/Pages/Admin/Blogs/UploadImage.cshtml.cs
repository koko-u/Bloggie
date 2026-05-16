using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bloggie.Web.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class UploadImage(
    ImagesService imagesService,
    [FromServices] IValidator<IFormFile> imageValidator
) : PageModel
{
    public async Task<IActionResult> OnPostAsync(
        IFormFile image,
        CancellationToken cancellationToken
    )
    {
        var result = await imageValidator.ValidateAsync(image, cancellationToken);
        if (!result.IsValid)
        {
            return BadRequest(result.ToString());
        }

        var imageRow = await imagesService.UploadImageAsync(image, cancellationToken);
        var altText = Path.GetFileNameWithoutExtension(image.FileName);

        return new JsonResult(
            new
            {
                Id = imageRow.Id,
                Url = imageRow.Url,
                AltText = altText,
            }
        );
    }
}
