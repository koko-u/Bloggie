using System;
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

        return rows.GroupByBlogPost().SingleOrDefault();
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
        var selectQuery = await File.ReadAllTextAsync(
            "Sql/BlogPosts/select_by_id.sql",
            cancellationToken
        );

        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var tx = await conn.BeginTransactionAsync(cancellationToken);

        // INSERT ROW
        var insertCmd = new CommandDefinition(
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
        var id = await conn.QuerySingleAsync<Guid>(insertCmd);

        // SELECT INSERTED ROW DATA
        var selectCmd = new CommandDefinition(
            commandText: selectQuery,
            parameters: new { id },
            transaction: tx,
            cancellationToken: cancellationToken
        );
        var rows = await conn.QueryAsync<BlogPostRow>(selectCmd);

        await tx.CommitAsync(cancellationToken);

        // GROUPING BlogPost data
        return rows.GroupByBlogPost().Single();
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

        return rows.GroupByBlogPost();
    }

    /// <summary>
    /// Get Blog Post Data by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<BlogPost?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var sqlQuery = await File.ReadAllTextAsync(
            "Sql/BlogPosts/select_by_id.sql",
            cancellationToken
        );

        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: sqlQuery,
            parameters: new { Id = id },
            cancellationToken: cancellationToken
        );
        var rows = await conn.QueryAsync<BlogPostRow>(cmd);

        return rows.GroupByBlogPost().SingleOrDefault();
    }

    public async Task<BlogPost?> UpdateAsync(EditBlogForm form, CancellationToken cancellationToken)
    {
        var updateQuery = await File.ReadAllTextAsync(
            "Sql/BlogPosts/update_by_id.sql",
            cancellationToken
        );
        var selectQuery = await File.ReadAllTextAsync(
            "Sql/BlogPosts/select_by_id.sql",
            cancellationToken
        );

        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var tx = await conn.BeginTransactionAsync(cancellationToken);

        // UPDATE ROW
        var updateCmd = new CommandDefinition(
            commandText: updateQuery,
            parameters: new
            {
                form.Id,
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
        var affectedRows = await conn.ExecuteAsync(updateCmd);
        if (affectedRows <= 0)
        {
            // no update rows
            return null;
        }

        // SELECT UPDATED ROW DATA
        var selectCmd = new CommandDefinition(
            commandText: selectQuery,
            parameters: new { form.Id },
            transaction: tx,
            cancellationToken: cancellationToken
        );
        var rows = await conn.QueryAsync<BlogPostRow>(selectCmd);

        await tx.CommitAsync(cancellationToken);

        // GROUPING BlogPost data
        return rows.GroupByBlogPost().SingleOrDefault();
    }
}
