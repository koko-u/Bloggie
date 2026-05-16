using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Bloggie.Web.Models.Rows;
using Bloggie.Web.Services;
using Bloggie.Web.Services.Tx;
using Dapper;
using Npgsql;

namespace Bloggie.Web.Repositories;

[AutoRegisterService]
public sealed class TagsRepository(NpgsqlDataSource dataSource, SqlResources sql)
{
    /// <summary>
    /// select all tags from tags table
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TagRow>> SelectAll(CancellationToken ct)
    {
        await using var conn = await dataSource.OpenConnectionAsync(ct);
        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("Tags/select_all.sql", ct),
            cancellationToken: ct
        );

        return await conn.QueryAsync<TagRow>(cmd);
    }

    /// <summary>
    /// Insert tag ids of blog id into blog_post_tags table
    /// </summary>
    /// <param name="session"></param>
    /// <param name="blogPostId"></param>
    /// <param name="tagIds"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<int> InsertTagIds(
        DbSession session,
        Guid blogPostId,
        List<Guid> tagIds,
        CancellationToken ct
    )
    {
        var (conn, tx) = session;

        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPostTags/insert_tag_ids.sql", ct),
            parameters: new { BlogPostId = blogPostId, TagIds = tagIds },
            transaction: tx,
            cancellationToken: ct
        );

        return await conn.ExecuteAsync(cmd);
    }

    /// <summary>
    /// Delete tag ids of blog id from blog_post_tags table
    /// </summary>
    /// <param name="session"></param>
    /// <param name="blogPostId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<int> DeleteTagIds(DbSession session, Guid blogPostId, CancellationToken ct)
    {
        var (conn, tx) = session;

        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPostTags/delete_tag_ids.sql", ct),
            parameters: new { BlogPostId = blogPostId },
            transaction: tx,
            cancellationToken: ct
        );

        return await conn.ExecuteAsync(cmd);
    }
}
