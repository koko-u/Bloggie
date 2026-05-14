using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggie.Web.FlashMessages;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Forms;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class List(BlogPostsService blogPostsService) : PageModel
{
    public List<BlogPost> BlogPosts { get; set; } = [];

    [BindProperty]
    public DeleteIdForm DeleteIdForm { get; set; } = new() { Id = Guid.Empty };

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        BlogPosts = (await blogPostsService.GetAllAsync(cancellationToken)).ToList();
    }

    public async Task<IActionResult> OnPostDeleteAsync(CancellationToken cancellationToken)
    {
        var id = DeleteIdForm.Id;
        var oldRow = await blogPostsService.GetByIdAsync(id, cancellationToken);
        if (oldRow is null)
        {
            TempData[FlashMessage.Key] = FlashMessage
                .Error($"Blog post with ID '{id}' not found")
                .ToJsonString();
            return RedirectToPage("/Admin/Blogs/List");
        }

        var deletedRow = await blogPostsService.DeleteAsync(id, cancellationToken);
        if (deletedRow is null)
        {
            TempData[FlashMessage.Key] = FlashMessage
                .Error($"Failed to update blog post with ID '{id}'")
                .ToJsonString();
            return RedirectToPage("/Admin/Blogs/List");
        }

        TempData[FlashMessage.Key] = FlashMessage
            .Success($"Blog post '{deletedRow.Heading}' deleted successfully")
            .ToJsonString();

        return RedirectToPage("/Admin/Blogs/List");
    }
}
