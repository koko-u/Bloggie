using Bloggie.Repo;
using Bloggie.Repo.Models.ViewModels;
using Bloggie.Web.Extensions;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class AddModel(IBlogPostsRepository blogPostsRepository) : PageModel
{
    [BindProperty]
    public AddBlogPost AddBlogPost { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        var created = await blogPostsRepository.CreateAsync(AddBlogPost);

        var notification = Notification.Success($"Blog Post [{created.Heading}] has been created.");
        TempData.Set(nameof(notification), notification);
        return RedirectToPage("/Admin/Blogs/Index");
    }
}
