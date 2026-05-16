using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Bloggie.Web.Models.Forms;
using Bloggie.Web.Models.Rows;
using Bloggie.Web.Services;
using Bloggie.Web.Services.Tx;
using Dapper;
using Npgsql;

namespace Bloggie.Web.Repositories;

/// <summary>
/// Blog Table Repository Service Class
/// </summary>
/// <param name="dataSource"></param>
/// <param name="sql"></param>
[AutoRegisterService]
public sealed class BlogPostsRepository(NpgsqlDataSource dataSource, SqlResources sql)
{
    /// <summary>
    /// SELECT ALL BlogPosts records with Tags and Images
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<IEnumerable<BlogPostRow>> SelectAll(CancellationToken ct)
    {
        await using var conn = await dataSource.OpenConnectionAsync(ct);
        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/select_all.sql", ct),
            cancellationToken: ct
        );
        return await conn.QueryAsync<BlogPostRow>(cmd);
    }

    /// <summary>
    /// SELECT BlogPost record by Id with Tags and Images
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<IEnumerable<BlogPostRow>> SelectById(Guid id, CancellationToken ct)
    {
        await using var conn = await dataSource.OpenConnectionAsync(ct);
        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/select_by_id.sql", ct),
            parameters: new { Id = id },
            cancellationToken: ct
        );
        return await conn.QueryAsync<BlogPostRow>(cmd);
    }

    public async Task<IEnumerable<BlogPostRow>> SelectBySlug(string slug, CancellationToken ct)
    {
        await using var conn = await dataSource.OpenConnectionAsync(ct);
        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/select_by_slug.sql", ct),
            parameters: new { Slug = slug },
            cancellationToken: ct
        );
        return await conn.QueryAsync<BlogPostRow>(cmd);
    }

    /// <summary>
    /// Insert new BlogPost Data
    /// </summary>
    /// <param name="session"></param>
    /// <param name="form"></param>
    /// <param name="ct"></param>
    /// <returns>Created BlogPost Id</returns>
    public async Task<Guid> Insert(DbSession session, AddBlogForm form, CancellationToken ct)
    {
        var (conn, tx) = session;

        // INSERT blog row
        var insertCmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/insert_one.sql", ct),
            parameters: new
            {
                form.Heading,
                form.PageTitle,
                Content = form.ContentMarkdown,
                form.ShortDescription,
                form.Slug,
                form.PublishedDate,
                form.Author,
                form.Visible,
            },
            transaction: tx,
            cancellationToken: ct
        );
        return await conn.QuerySingleAsync<Guid>(insertCmd);
    }

    public async Task<int> UpdateById(DbSession session, EditBlogForm form, CancellationToken ct)
    {
        var (conn, tx) = session;

        // UPDATE ROW
        var updateCmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/update_by_id.sql", ct),
            parameters: new
            {
                form.Id,
                form.Heading,
                form.PageTitle,
                Content = form.ContentMarkdown,
                form.ShortDescription,
                form.Slug,
                form.PublishedDate,
                form.Author,
                form.Visible,
            },
            transaction: tx,
            cancellationToken: ct
        );
        return await conn.ExecuteAsync(updateCmd);
    }

    public async Task<BlogPostRow?> DeleteById(DbSession session, Guid id, CancellationToken ct)
    {
        var (conn, tx) = session;

        // DELETE ROW
        var deleteCmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/delete_by_id.sql", ct),
            parameters: new { Id = id },
            transaction: tx,
            cancellationToken: ct
        );
        return await conn.QuerySingleOrDefaultAsync<BlogPostRow>(deleteCmd);
    }
}
