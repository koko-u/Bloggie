using AutoMapper;
using Bloggie.Db.Data;
using Bloggie.Db.Models.Domain;
using Bloggie.Web.Models.ViewModels;
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
}
