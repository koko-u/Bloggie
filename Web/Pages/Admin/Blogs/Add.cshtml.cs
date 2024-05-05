using Bloggie.Repo;
using Bloggie.Repo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class AddModel(IBlogPostsRepository blogPostsRepository) : PageModel
{
    [BindProperty]
    public AddBlogPost AddBlogPost { get; set; } = new();

    [TempData]
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var created = await blogPostsRepository.CreateAsync(AddBlogPost);

        SuccessMessage = $"Blog Post [{created.Heading}] has been created.";
        return RedirectToPage("/Admin/Blogs/Index");
    }
}
