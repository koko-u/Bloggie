using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Bloggie.Web.Mappings;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Forms;
using Bloggie.Web.Models.Rows;
using Bloggie.Web.Utils;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Bloggie.Web.Services;

/// <summary>
/// Blog Posts Operation Service
/// </summary>
/// <param name="dataSource"></param>
[AutoRegisterService]
public sealed class BlogPostsService(
    SqlResources sql,
    NpgsqlDataSource dataSource,
    ILogger<BlogPostsService> logger
)
{
    /// <summary>
    /// Get all blog posts
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<BlogPost>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/select_all.sql", cancellationToken),
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
        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/select_by_id.sql", cancellationToken),
            parameters: new { Id = id },
            cancellationToken: cancellationToken
        );
        var rows = await conn.QueryAsync<BlogPostRow>(cmd);

        return rows.GroupByBlogPost().SingleOrDefault();
    }

    /// <summary>
    /// Get Blog Post by slug
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/select_by_slug.sql", cancellationToken),
            parameters: new { slug },
            cancellationToken: cancellationToken
        );
        var rows = await conn.QueryAsync<BlogPostRow>(cmd);

        return rows.GroupByBlogPost().SingleOrDefault();
    }

    /// <summary>
    /// Create a new blog post and associate images
    /// </summary>
    /// <param name="form"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<BlogPost> CreateAsync(AddBlogForm form, CancellationToken cancellationToken)
    {
        // update operation
        var blogPostId = await InsertAndUpdateImagesAsync(form, cancellationToken);

        // SELECT INSERTED ROW DATA
        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var selectCmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/select_by_id.sql", cancellationToken),
            parameters: new { id = blogPostId },
            cancellationToken: cancellationToken
        );
        var rows = await conn.QueryAsync<BlogPostRow>(selectCmd);

        // GROUPING BlogPost data
        return rows.GroupByBlogPost().Single();
    }

    /// <summary>
    /// Update a blog post with the provided form data.
    /// </summary>
    /// <param name="form"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<BlogPost?> UpdateAsync(EditBlogForm form, CancellationToken cancellationToken)
    {
        var blogPostId = await UpdateAndAssociateImagesAsync(form, cancellationToken);
        if (blogPostId is null)
        {
            return null;
        }

        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        // SELECT UPDATED ROW DATA
        var selectCmd = new CommandDefinition(
            commandText: await sql.GetAsync("BlogPosts/select_by_id.sql", cancellationToken),
            parameters: new { Id = blogPostId },
            cancellationToken: cancellationToken
        );
        var rows = await conn.QueryAsync<BlogPostRow>(selectCmd);

        // GROUPING BlogPost data
        return rows.GroupByBlogPost().SingleOrDefault();
    }

    /// <summary>
    /// Delete Blog Post by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<BlogPost?> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var tx = await conn.BeginTransactionAsync(cancellationToken);

        try
        {
            // DELETE ROW
            var deleteCmd = new CommandDefinition(
                commandText: await sql.GetAsync("BlogPosts/delete_by_id.sql", cancellationToken),
                parameters: new { Id = id },
                transaction: tx,
                cancellationToken: cancellationToken
            );
            var rows = await conn.QuerySingleOrDefaultAsync<BlogPostRow>(deleteCmd);

            await tx.CommitAsync(cancellationToken);

            // GROUPING BlogPost data
            return rows?.ToBlogPostModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting blog post with id {Id}", id);

            await tx.RollbackAsync(cancellationToken);

            throw;
        }
    }

    /// <summary>
    /// Insert new BlogPost
    /// </summary>
    /// <param name="form"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<Guid> InsertAndUpdateImagesAsync(
        AddBlogForm form,
        CancellationToken cancellationToken
    )
    {
        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var tx = await conn.BeginTransactionAsync(cancellationToken);
        try
        {
            // INSERT blog row
            var insertCmd = new CommandDefinition(
                commandText: await sql.GetAsync("BlogPosts/insert_one.sql", cancellationToken),
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
                cancellationToken: cancellationToken
            );
            var blogPostId = await conn.QuerySingleAsync<Guid>(insertCmd);

            // image ids (contained in markdown content urls)
            if (form.ContentMarkdown is not null)
            {
                // ブログポストの本文にある画像のURL
                var urls = form.ContentMarkdown.ExtractImageUrls().ToArray();
                var updateCmd = new CommandDefinition(
                    commandText: await sql.GetAsync(
                        "Images/update_blog_post_id.sql",
                        cancellationToken
                    ),
                    parameters: new
                    {
                        BlogPostId = blogPostId,
                        form.ImageIds,
                        ImageUrls = urls,
                    },
                    transaction: tx,
                    cancellationToken: cancellationToken
                );
                await conn.ExecuteAsync(updateCmd);

                // 本文にない画像のURLレコードは削除する
                var deleteQuery = await sql.GetAsync(
                    "Images/delete_no_content.sql",
                    cancellationToken
                );
                var deleteCmd = new CommandDefinition(
                    commandText: deleteQuery,
                    parameters: new
                    {
                        BlogPostId = blogPostId,
                        form.ImageIds,
                        ImageUrls = urls,
                    },
                    transaction: tx,
                    cancellationToken: cancellationToken
                );
                await conn.ExecuteAsync(deleteCmd);
            }

            // Commit data
            await tx.CommitAsync(cancellationToken);

            return blogPostId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating blog post");

            await tx.RollbackAsync(cancellationToken);

            throw;
        }
    }

    /// <summary>
    /// Update and associate images for a blog post.
    /// </summary>
    /// <param name="form"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<Guid?> UpdateAndAssociateImagesAsync(
        EditBlogForm form,
        CancellationToken cancellationToken
    )
    {
        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var tx = await conn.BeginTransactionAsync(cancellationToken);
        try
        {
            // UPDATE ROW
            var updateCmd = new CommandDefinition(
                commandText: await sql.GetAsync("BlogPosts/update_by_id.sql", cancellationToken),
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
                cancellationToken: cancellationToken
            );
            var affectedRows = await conn.ExecuteAsync(updateCmd);

            // ブログの本文に含まれる画像Url
            var urls = form.ContentMarkdown?.ExtractImageUrls().ToArray() ?? [];
            // アップロードされた画像にブログIDを紐づける
            var updateImgCmd = new CommandDefinition(
                commandText: await sql.GetAsync(
                    "Images/update_blog_post_id.sql",
                    cancellationToken
                ),
                parameters: new
                {
                    BlogPostId = form.Id,
                    form.ImageIds,
                    ImageUrls = urls,
                },
                transaction: tx,
                cancellationToken: cancellationToken
            );
            await conn.ExecuteAsync(updateImgCmd);

            // アップロードしたけど、本文にはもうない行を削除する
            var deleteImgCmd = new CommandDefinition(
                commandText: await sql.GetAsync("Images/delete_no_content.sql", cancellationToken),
                parameters: new
                {
                    BlogPostId = form.Id,
                    form.ImageIds,
                    ImageUrls = urls,
                },
                transaction: tx,
                cancellationToken: cancellationToken
            );
            await conn.ExecuteAsync(deleteImgCmd);

            // ブログ本文から削除された画像のデータを削除する
            var deleteImgCmd2 = new CommandDefinition(
                commandText: await sql.GetAsync(
                    "Images/delete_deleted_content.sql",
                    cancellationToken
                ),
                parameters: new { BlogPostId = form.Id, ImageUrls = urls },
                transaction: tx,
                cancellationToken: cancellationToken
            );
            await conn.ExecuteAsync(deleteImgCmd2);

            await tx.CommitAsync(cancellationToken);

            return affectedRows > 0 ? form.Id : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to commit transaction for blog post update");
            await tx.RollbackAsync(cancellationToken);

            throw;
        }
    }
}
