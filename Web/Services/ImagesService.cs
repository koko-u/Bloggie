using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Bloggie.Web.Models.Rows;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Npgsql;

namespace Bloggie.Web.Services;

[AutoRegisterService]
public sealed class ImagesService(IWebHostEnvironment env, NpgsqlDataSource dataSource)
{
    public async Task<ImageRow> UploadImageAsync(
        IFormFile image,
        CancellationToken cancellationToken
    )
    {
        var insertSql = await File.ReadAllTextAsync(
            "Sql/Images/insert_without_blog_post.sql",
            cancellationToken
        );

        await using var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var tx = await conn.BeginTransactionAsync(cancellationToken);

        var (id, url) = await SaveUploadFileAsync(image, cancellationToken);

        var insertCmd = new CommandDefinition(
            commandText: insertSql,
            parameters: new { Id = id, Url = url },
            transaction: tx,
            cancellationToken: cancellationToken
        );

        await conn.ExecuteAsync(insertCmd);

        await tx.CommitAsync(cancellationToken);

        return new ImageRow { Id = id, Url = url };
    }

    private async Task<(Guid id, string url)> SaveUploadFileAsync(
        IFormFile image,
        CancellationToken cancellationToken
    )
    {
        var now = DateTime.UtcNow;

        // Directories
        var relativeDir = $"uploads/images/{now:yyyy}/{now:MM}/";
        var absoluteDir = Path.Combine(env.WebRootPath, relativeDir);
        if (!Directory.Exists(absoluteDir))
        {
            Directory.CreateDirectory(absoluteDir);
        }

        // image id and filename
        var id = Guid.CreateVersion7();
        var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
        var name = $"{id:D}{ext}";
        var absolutePath = Path.Combine(absoluteDir, name);

        await using var stream = new FileStream(
            absolutePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 4096,
            useAsync: true
        );
        await image.CopyToAsync(stream, cancellationToken);

        var url = "/" + Path.Combine(relativeDir, name).Replace("\\", "/");

        return (id, url);
    }
}
