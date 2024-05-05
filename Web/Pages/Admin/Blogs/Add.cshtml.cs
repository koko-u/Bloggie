using AutoMapper;
using Bloggie.Db.Data;
using Bloggie.Db.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class AddModel(BloggieDbContext dbContext, IMapper mapper) : PageModel
{
    [BindProperty]
    public AddBlogPost AddBlogPost { get; set; } = new();

    public async Task<IActionResult> OnPost()
    {
        var blogPost = new BlogPost();
        mapper.Map(AddBlogPost, blogPost);

        await dbContext.BlogPosts.AddAsync(blogPost);
        await dbContext.SaveChangesAsync();

        return RedirectToPage("/Admin/Blogs/Index");
    }
}
