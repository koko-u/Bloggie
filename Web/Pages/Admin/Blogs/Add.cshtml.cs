using System.Threading;
using System.Threading.Tasks;
using Bloggie.Web.Extensions;
using Bloggie.Web.FlashMessages;
using Bloggie.Web.Models.Forms;
using Bloggie.Web.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

/// <summary>
/// Create New Blog Post Page
/// </summary>
/// <param name="blogPostsService"></param>
public sealed class Add(BlogPostsService blogPostsService) : PageModel
{
    /// <summary>
    /// Blog Post Form Data
    /// </summary>
    [BindProperty]
    public AddBlogForm Blog { get; set; } = new();

    /// <summary>
    /// Show New Blog Post Form
    /// </summary>
    /// <returns></returns>
    public Task OnGetAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Create New Blog Post
    /// </summary>
    /// <param name="validator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

        var blogPost = await blogPostsService.CreateAsync(this.Blog, cancellationToken);
        var successMessage = FlashMessage.Success(
            $"Blog post '{blogPost.Heading}' created successfully"
        );
        TempData[FlashMessage.Key] = successMessage.ToJsonString();

        return RedirectToPage("/Admin/Blogs/List");
    }
}
