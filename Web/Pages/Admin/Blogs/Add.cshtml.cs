using Bloggie.Repo;
using Bloggie.Repo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class AddModel(IBlogPostsRepository blogPostsRepository) : PageModel
{
    [BindProperty]
    public AddBlogPost AddBlogPost { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        await blogPostsRepository.CreateAsync(AddBlogPost);

        return RedirectToPage("/Admin/Blogs/Index");
    }
}
