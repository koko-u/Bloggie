using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggie.Web.Extensions;
using Bloggie.Web.FlashMessages;
using Bloggie.Web.Models.Domain;
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
public sealed class Add(BlogPostsService blogPostsService, TagsService tagsService) : PageModel
{
    /// <summary>
    /// Blog Post Form Data
    /// </summary>
    [BindProperty]
    public AddBlogForm Blog { get; set; } = new();

    public IReadOnlyList<Tag> Tags { get; set; } = [];

    /// <summary>
    /// Show New Blog Post Form
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        var result = await tagsService.GetAllAsync(ct);
        Tags = result.ToList();

        return Page();
    }

    /// <summary>
    /// Create New Blog Post
    /// </summary>
    /// <param name="validator"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostAsync(
        [FromServices] IValidator<AddBlogForm> validator,
        CancellationToken ct
    )
    {
        var result = await validator.ValidateAsync(this.Blog, ct);
        if (!result.IsValid)
        {
            ModelState.AddModelErrorFrom(result.Errors, nameof(Blog));
            return Page();
        }

        var blogPost = await blogPostsService.CreateAsync(this.Blog, ct);
        var successMessage = FlashMessage.Success(
            $"Blog post '{blogPost.Heading}' created successfully"
        );
        TempData[FlashMessage.Key] = successMessage.ToJsonString();

        return RedirectToPage("/Admin/Blogs/List");
    }
}
