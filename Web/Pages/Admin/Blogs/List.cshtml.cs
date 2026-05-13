using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class List(BlogPostsService blogPostsService) : PageModel
{
    public List<BlogPost> BlogPosts { get; set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        BlogPosts = (await blogPostsService.GetAllAsync(cancellationToken)).ToList();
    }
}
