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
public sealed class BlogPostsService(NpgsqlDataSource dataSource)
{
    public async Task<BlogPost?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default
    )
    {
        var sqlQuery = await File.ReadAllTextAsync(
            "Sql/BlogPosts/select_by_slug.sql",
            cancellationToken
        );

        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: sqlQuery,
            parameters: new { slug },
            cancellationToken: cancellationToken
        );
        var rows = await conn.QueryAsync<BlogPostRow>(cmd);

        return rows.GroupBy(r => r.ToBlogPostModel())
            .Select(g =>
            {
                var blogPost = g.Key.FastDeepClone();
                blogPost.Tags = g.Where(r => r.TagId.HasValue).Select(r => r.ToTagModel()).ToList();

                return blogPost;
            })
            .SingleOrDefault();
    }
}
