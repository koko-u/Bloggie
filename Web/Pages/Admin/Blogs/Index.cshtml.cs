using Bloggie.Repo;
using Bloggie.Repo.Models.ViewModels;
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
        if (deletedBlogPost is null)
        {
            // TODO: add not found message.
            return RedirectToPage("/Admin/Blogs/Index");
        }

        return RedirectToPage("/Admin/Blogs/Index");
    }
}
