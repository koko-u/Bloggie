using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggie.Web.Extensions;
using Bloggie.Web.FlashMessages;
using Bloggie.Web.Mappings;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Forms;
using Bloggie.Web.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class Edit(BlogPostsService blogPostsService, TagsService tagsService) : PageModel
{
    [BindProperty]
    public EditBlogForm Blog { get; set; } = new() { Id = Guid.Empty };

    public IReadOnlyList<Tag> Tags { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        var result = await tagsService.GetAllAsync(ct);
        Tags = result.ToList();

        var blogPost = await blogPostsService.GetByIdAsync(id, ct);
        if (blogPost is null)
        {
            TempData[FlashMessage.Key] = FlashMessage.Error("Blog post not found").ToJsonString();
            return RedirectToPage("/Admin/Blogs/List");
        }

        // Content value is Raw markdown content
        Blog = blogPost.ToEditBlogForm();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IValidator<EditBlogForm> validator,
        CancellationToken ct
    )
    {
        var oldData = await blogPostsService.GetByIdAsync(Blog.Id, ct);
        if (oldData is null)
        {
            TempData[FlashMessage.Key] = FlashMessage
                .Error($"Blog post with ID '{Blog.Id}' not found")
                .ToJsonString();
            return RedirectToPage("/Admin/Blogs/List");
        }

        var result = await validator.ValidateAsync(this.Blog, ct);
        if (!result.IsValid)
        {
            ModelState.AddModelErrorFrom(result.Errors, nameof(Blog));
            return Page();
        }

        var blogPost = await blogPostsService.UpdateAsync(this.Blog, ct);
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
