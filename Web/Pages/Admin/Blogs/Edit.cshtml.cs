using AutoMapper;
using Bloggie.Db.Data;
using Bloggie.Db.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class EditModel(BloggieDbContext dbContext, IMapper mapper) : PageModel
{
    [BindProperty]
    public required BlogPostRow BlogPost { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var blogPost = await dbContext.BlogPosts.SingleOrDefaultAsync(blogPost => blogPost.Id == id);
        if (blogPost is null)
        {
            return RedirectToPage("/Admin/Blogs/Index");
        }

        BlogPost = mapper.Map<BlogPost, BlogPostRow>(blogPost);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        var dbBlogPost = await dbContext.BlogPosts.SingleOrDefaultAsync(blogPost => blogPost.Id == id);
        if (dbBlogPost is null)
        {
            return RedirectToPage("/Admin/Blogs/Index");
        }

        mapper.Map(BlogPost, dbBlogPost);
        await dbContext.SaveChangesAsync();

        return RedirectToPage("/Admin/Blogs/Index");
    }
}
