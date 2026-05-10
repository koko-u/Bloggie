using System.Threading;
using System.Threading.Tasks;
using Bloggie.Web.Extensions;
using Bloggie.Web.Models.Forms;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public sealed class Add : PageModel
{
    [BindProperty]
    public AddBlogForm Blog { get; set; } = new();

    public Task OnGetAsync()
    {
        return Task.CompletedTask;
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IValidator<AddBlogForm> validator,
        CancellationToken cancellationToken
    )
    {
        var result = await validator.ValidateAsync(this.Blog, cancellationToken);
        if (!result.IsValid)
        {
            ModelState.AddModelErrorFrom(result.Errors, nameof(Blog));
            return Page();
        }

        return RedirectToPage("/Admin/Blogs/Index");
    }
}
