using Bloggie.Repo;
using Bloggie.Repo.Models.ViewModels;
using Bloggie.Web.Extensions;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class EditModel(IBlogPostsRepository blogPostsRepository) : PageModel
{
    [BindProperty]
    public required BlogPostRow BlogPost { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var blogPost = await blogPostsRepository.GetByIdAsync(id);
        if (blogPost.IsErr(out var errorMessage))
        {
            var errorNotification = Notification.Error(errorMessage.Message);
            TempData.Set("notification", errorNotification);
            return RedirectToPage("/Admin/Blogs/Index");
        }

        BlogPost = blogPost.UnwrapOrElse(err => throw new Exception(err.Message));
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        var updatedBlogPost = await blogPostsRepository.UpdateAsync(id, BlogPost);
        if (updatedBlogPost.IsErr(out var errorMessage))
        {
            var errorNotification = Notification.Error(errorMessage.Message);
            TempData.Set("notification", errorNotification);
            return RedirectToPage("/Admin/Blogs/Index");
        }

        var notification = Notification.Success($"Blog Post [{updatedBlogPost.Unwrap().Heading}] has been updated.");
        TempData.Set(nameof(notification), notification);
        return RedirectToPage("/Admin/Blogs/Index");
    }
}
