using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages;

public class IndexModel : PageModel
{
    public Task OnGetAsync(CancellationToken cancelToken)
    {
        return Task.FromResult(Task.CompletedTask);
    }
}
