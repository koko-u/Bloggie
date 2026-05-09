using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages;

public class IndexModel(TagsService tagsService) : PageModel
{
    public IEnumerable<Tag> Tags { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancelToken)
    {
        Tags = await tagsService.GetAllTags(cancelToken);
    }
}
