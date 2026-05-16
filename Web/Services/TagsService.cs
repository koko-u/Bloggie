using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Bloggie.Web.Mappings;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;

namespace Bloggie.Web.Services;

[AutoRegisterService]
public sealed class TagsService(TagsRepository tagsRepo)
{
    /// <summary>
    /// Get All Tag Data
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken ct)
    {
        var rows = await tagsRepo.SelectAll(ct);
        return rows.Select(r => r.ToTagModel());
    }
}
