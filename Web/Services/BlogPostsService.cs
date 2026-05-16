using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Bloggie.Web.Mappings;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Forms;
using Bloggie.Web.Repositories;
using Bloggie.Web.Services.Tx;
using Bloggie.Web.Utils;

namespace Bloggie.Web.Services;

/// <summary>
/// Blog Posts Operation Service
/// </summary>
/// <param name="txRunner"></param>
/// <param name="blogPostsRepo"></param>
/// <param name="imagesRepo"></param>
[AutoRegisterService]
public sealed class BlogPostsService(
    TransactionRunner txRunner,
    BlogPostsRepository blogPostsRepo,
    ImagesRepository imagesRepo,
    TagsRepository tagsRepo
)
{
    /// <summary>
    /// Get all blog posts
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<IEnumerable<BlogPost>> GetAllAsync(CancellationToken ct)
    {
        var rows = await blogPostsRepo.SelectAll(ct);
        return rows.GroupByBlogPost();
    }

    /// <summary>
    /// Get Blog Post Data by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<BlogPost?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var rows = await blogPostsRepo.SelectById(id, ct);
        return rows.GroupByBlogPost().SingleOrDefault();
    }

    /// <summary>
    /// Get Blog Post by slug
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken ct)
    {
        var rows = await blogPostsRepo.SelectBySlug(slug, ct);
        return rows.GroupByBlogPost().SingleOrDefault();
    }

    /// <summary>
    /// Create a new blog post and associate images
    /// </summary>
    /// <param name="form"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<BlogPost> CreateAsync(AddBlogForm form, CancellationToken ct)
    {
        // update operation
        var blogPostId = await txRunner.ExecuteAsync(
            async (session, ctn) =>
            {
                // create blog post
                var blogPostId = await blogPostsRepo.Insert(session, form, ctn);
                // associate tags
                await tagsRepo.InsertTagIds(session, blogPostId, form.TagIds, ctn);

                if (form.ContentMarkdown is null)
                {
                    return blogPostId;
                }

                // ブログポストの本文にある画像のURL
                var urls = form.ContentMarkdown.ExtractImageUrls().ToList();

                // アップロードした画像テーブルにブログIDを更新する
                await imagesRepo.UpdateBlogPostId(session, form.ImageIds, urls, blogPostId, ctn);

                // 本文にない画像のURLレコードは削除する
                await imagesRepo.DeleteDanglingImages(session, form.ImageIds, urls, ctn);

                return blogPostId;
            },
            ct
        );

        // SELECT INSERTED ROW DATA
        var rows = await blogPostsRepo.SelectById(blogPostId, ct);

        // GROUPING BlogPost data
        return rows.GroupByBlogPost().Single();
    }

    /// <summary>
    /// Update a blog post with the provided form data.
    /// </summary>
    /// <param name="form"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<BlogPost?> UpdateAsync(EditBlogForm form, CancellationToken ct)
    {
        var affectedRows = await txRunner.ExecuteAsync(
            async (session, ctn) =>
            {
                // ブログを更新する
                var affectedRows = await blogPostsRepo.UpdateById(session, form, ctn);
                // いったんタグの関連を切る
                await tagsRepo.DeleteTagIds(session, form.Id, ctn);
                // 改めてタグを関連づける
                await tagsRepo.InsertTagIds(session, form.Id, form.TagIds, ctn);

                // ブログの本文に含まれる画像Url
                var urls = form.ContentMarkdown?.ExtractImageUrls().ToList() ?? [];

                // アップロードした画像に対してブログIDを更新する
                await imagesRepo.UpdateBlogPostId(session, form.ImageIds, urls, form.Id, ctn);

                // アップロードしたけど、本文にはもうない行を削除する
                await imagesRepo.DeleteDanglingImages(session, form.ImageIds, urls, ctn);

                // ブログ本文から削除された画像のデータを削除する
                await imagesRepo.DeleteAccordingRemoveContent(session, urls, form.Id, ctn);

                return affectedRows;
            },
            ct
        );

        if (affectedRows <= 0)
        {
            return null;
        }

        var rows = await blogPostsRepo.SelectById(form.Id, ct);
        return rows.GroupByBlogPost().SingleOrDefault();
    }

    /// <summary>
    /// Delete Blog Post by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<BlogPost?> DeleteAsync(Guid id, CancellationToken ct)
    {
        var row = await txRunner.ExecuteAsync(
            async (session, ctn) => await blogPostsRepo.DeleteById(session, id, ctn),
            ct
        );
        return row?.ToBlogPostModel();
    }
}
