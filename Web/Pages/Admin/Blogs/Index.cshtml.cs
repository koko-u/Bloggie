using AutoMapper;
using Bloggie.Db.Data;
using Bloggie.Db.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class IndexModel(BloggieDbContext dbContext, IMapper mapper) : PageModel
{
    public List<BlogPostRow> BlogPosts { get; set; } = [];

    public async Task OnGet()
    {
        var blogPosts = await dbContext.BlogPosts.ToListAsync();
        BlogPosts = mapper.Map<List<BlogPost>, List<BlogPostRow>>(blogPosts);
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var dbBlogPost = await dbContext.BlogPosts.SingleOrDefaultAsync(blogPost => blogPost.Id == id);
        if (dbBlogPost is null)
        {
            return RedirectToPage("/Admin/Blogs/Index");
        }

        dbContext.BlogPosts.Remove(dbBlogPost);
        await dbContext.SaveChangesAsync();

        return RedirectToPage("/Admin/Blogs/Index");
    }
}
