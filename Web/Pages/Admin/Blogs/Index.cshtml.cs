using Bloggie.Repo;
using Bloggie.Repo.Models.ViewModels;
using Bloggie.Web.Extensions;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class IndexModel(IBlogPostsRepository blogPostsRepository)
    : PageModel
{
    public IEnumerable<BlogPostRow> BlogPosts { get; set; } = [];

    public async Task OnGetAsync()
    {
        var blogPosts = await blogPostsRepository.GetAllAsync();
        BlogPosts = blogPosts;
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var deletedBlogPost = await blogPostsRepository.DeleteByIdAsync(id);
        if (deletedBlogPost.IsErr(out var errorMessage))
        {
            var errorNotification = Notification.Error(errorMessage.Message);
            TempData.Set("notification", errorNotification);
            return RedirectToPage("/Admin/Blogs/Index");
        }

        var notification = Notification.Info($"Blog Post [{deletedBlogPost.Unwrap().Heading}] has been deleted.");
        TempData.Set(nameof(notification), notification);
        return RedirectToPage("/Admin/Blogs/Index");
    }
}
