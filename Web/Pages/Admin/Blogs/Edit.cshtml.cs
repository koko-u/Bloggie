using System;
using System.Threading;
using System.Threading.Tasks;
using Bloggie.Web.Extensions;
using Bloggie.Web.FlashMessages;
using Bloggie.Web.Mappings;
using Bloggie.Web.Models.Forms;
using Bloggie.Web.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class Edit(BlogPostsService blogPostsService) : PageModel
{
    [BindProperty]
    public EditBlogForm Blog { get; set; } = new() { Id = Guid.Empty };

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        var blogPost = await blogPostsService.GetByIdAsync(id, cancellationToken);
        if (blogPost is null)
        {
            TempData[FlashMessage.Key] = FlashMessage.Error("Blog post not found").ToJsonString();
            return RedirectToPage("/Admin/Blogs/List");
        }

        Blog = blogPost.ToEditBlogForm();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IValidator<EditBlogForm> validator,
        CancellationToken cancellationToken
    )
    {
        var oldData = await blogPostsService.GetByIdAsync(Blog.Id, cancellationToken);
        if (oldData is null)
        {
            TempData[FlashMessage.Key] = FlashMessage
                .Error($"Blog post with ID '{Blog.Id}' not found")
                .ToJsonString();
            return RedirectToPage("/Admin/Blogs/List");
        }

        var result = await validator.ValidateAsync(this.Blog, cancellationToken);
        if (!result.IsValid)
        {
            ModelState.AddModelErrorFrom(result.Errors, nameof(Blog));
            return Page();
        }

        var blogPost = await blogPostsService.UpdateAsync(this.Blog, cancellationToken);
        if (blogPost is null)
        {
            TempData[FlashMessage.Key] = FlashMessage
                .Error($"Failed to update blog post '{Blog.Heading}'")
                .ToJsonString();
            return Page();
        }

        TempData[FlashMessage.Key] = FlashMessage
            .Success($"Blog post '{blogPost.Heading}' updated successfully")
            .ToJsonString();

        return RedirectToPage("/Admin/Blogs/List");
    }
}
