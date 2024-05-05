using Bloggie.Repo;
using Bloggie.Repo.Models.ViewModels;
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
            // TODO: add not found message.
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
            // TODO: add not found message.
            return RedirectToPage("/Admin/Blogs/Index");
        }

        return RedirectToPage("/Admin/Blogs/Index");
    }
}
