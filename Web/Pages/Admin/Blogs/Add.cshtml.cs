using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class AddModel : PageModel
{
    [BindProperty]
    public AddBlogPost AddBlogPost { get; set; } = new();

    public void OnPost()
    {
        var heading = AddBlogPost.Heading;
        var pageTitle = AddBlogPost.PageTitle;

    }
}
