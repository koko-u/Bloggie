using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Bloggie.Web.Mappings;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Forms;
using Bloggie.Web.Models.Rows;
using Dapper;
using Npgsql;

namespace Bloggie.Web.Services;

/// <summary>
/// Blog Posts Operation Service
/// </summary>
/// <param name="dataSource"></param>
[AutoRegisterService]
public sealed class BlogPostsService(NpgsqlDataSource dataSource)
{
    /// <summary>
    /// Get Blog Post by slug
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Create a new blog post
    /// </summary>
    /// <param name="form"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<BlogPost> CreateAsync(
        AddBlogForm form,
        CancellationToken cancellationToken = default
    )
    {
        var insertQuery = await File.ReadAllTextAsync(
            "Sql/BlogPosts/insert_one.sql",
            cancellationToken
        );

        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var tx = await conn.BeginTransactionAsync(cancellationToken);

        var cmd = new CommandDefinition(
            commandText: insertQuery,
            parameters: new
            {
                form.Heading,
                form.PageTitle,
                form.Content,
                form.ShortDescription,
                form.FeaturedImageUrl,
                form.Slug,
                form.PublishedDate,
                form.Author,
                form.Visible,
            },
            transaction: tx,
            cancellationToken: cancellationToken
        );
        var row = await conn.QuerySingleAsync<BlogPostRow>(cmd);
        await tx.CommitAsync(cancellationToken);

        return row.ToBlogPostModel();
    }

    /// <summary>
    /// Get all blog posts
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<BlogPost>> GetAllAsync(CancellationToken cancellationToken)
    {
        var sqlQuery = await File.ReadAllTextAsync(
            "Sql/BlogPosts/select_all.sql",
            cancellationToken
        );

        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: sqlQuery,
            cancellationToken: cancellationToken
        );
        var rows = await conn.QueryAsync<BlogPostRow>(cmd);

        return rows.GroupBy(r => r.ToBlogPostModel())
            .Select(g =>
            {
                var blogPost = g.Key.FastDeepClone();
                blogPost.Tags = g.Where(r => r.TagId.HasValue).Select(r => r.ToTagModel()).ToList();

                return blogPost;
            });
    }
}
