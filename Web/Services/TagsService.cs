using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Bloggie.Web.Mappings;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Rows;
using Dapper;
using Npgsql;

namespace Bloggie.Web.Services;

[AutoRegisterService]
public sealed class TagsService(NpgsqlDataSource dataSource)
{
    public async Task<IEnumerable<Tag>> GetAllTags(CancellationToken cancellationToken = default)
    {
        var selectAllSql = await File.ReadAllTextAsync(
            "Sql/Tags/select_all.sql",
            cancellationToken
        );
        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: selectAllSql,
            cancellationToken: cancellationToken
        );

        var rows = await conn.QueryAsync<TagRow>(cmd);

        return rows.GroupBy(r => r.ToTagModel())
            .Select(g =>
            {
                return new Tag
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    BlogPosts = g.Where(r => r.BlogPostId.HasValue)
                        .Select(r => r.ToBlogPostModel())
                        .ToList(),
                };
            });
    }
}
