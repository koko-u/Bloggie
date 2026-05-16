using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Bloggie.Web.Services;
using Bloggie.Web.Services.Tx;
using Dapper;

namespace Bloggie.Web.Repositories;

[AutoRegisterService]
public sealed class ImagesRepository(SqlResources sql)
{
    /// <summary>
    /// 画像テーブルにあるアップロード済みの画像ID行にブログIDを設定します。ただしブログ本文に含まれない画像は除く
    /// </summary>
    /// <param name="session">データベースセッションです</param>
    /// <param name="uploadedImageIds">アップロード済みの画像IDのリストです</param>
    /// <param name="blogContentUrls">ブログ本文に含まれる画像URLのリストです</param>
    /// <param name="blogPostId">ブログIDです</param>
    /// <param name="ct">キャンセルトークンです</param>
    /// <returns>更新された行数</returns>
    public async Task<int> UpdateBlogPostId(
        DbSession session,
        List<Guid> uploadedImageIds,
        List<string> blogContentUrls,
        Guid blogPostId,
        CancellationToken ct
    )
    {
        var (conn, tx) = session;

        // アップロードされた画像にブログIDを紐づける
        var updateImgCmd = new CommandDefinition(
            commandText: await sql.GetAsync("Images/update_blog_post_id.sql", ct),
            parameters: new
            {
                BlogPostId = blogPostId,
                ImageIds = uploadedImageIds,
                ImageUrls = blogContentUrls,
            },
            transaction: tx,
            cancellationToken: ct
        );
        return await conn.ExecuteAsync(updateImgCmd);
    }

    /// <summary>
    /// アップロードした画像の中から、ブログの本文から削除された画像を削除する
    /// </summary>
    /// <param name="session"></param>
    /// <param name="uploadedImageIds"></param>
    /// <param name="blogContentUrls"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<int> DeleteDanglingImages(
        DbSession session,
        List<Guid> uploadedImageIds,
        List<string> blogContentUrls,
        CancellationToken ct
    )
    {
        var (conn, tx) = session;

        var deleteImgCmd = new CommandDefinition(
            commandText: await sql.GetAsync("Images/delete_dangling_rows.sql", ct),
            parameters: new { ImageIds = uploadedImageIds, ImageUrls = blogContentUrls },
            transaction: tx,
            cancellationToken: ct
        );
        return await conn.ExecuteAsync(deleteImgCmd);
    }

    /// <summary>
    /// ブログに紐づけられた画像から、本文から削除された画像を削除します
    /// </summary>
    /// <param name="session"></param>
    /// <param name="blogContentUrls"></param>
    /// <param name="blogPostId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<int> DeleteAccordingRemoveContent(
        DbSession session,
        List<string> blogContentUrls,
        Guid blogPostId,
        CancellationToken ct
    )
    {
        var (conn, tx) = session;

        var deleteImgCmd2 = new CommandDefinition(
            commandText: await sql.GetAsync("Images/delete_deleted_content.sql", ct),
            parameters: new { BlogPostId = blogPostId, ImageUrls = blogContentUrls },
            transaction: tx,
            cancellationToken: ct
        );
        return await conn.ExecuteAsync(deleteImgCmd2);
    }
}
