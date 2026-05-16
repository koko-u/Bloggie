using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Bloggie.Web.Services;

[AutoRegisterService]
public sealed class SqlResources(IWebHostEnvironment env)
{
    private readonly IFileProvider _fileProvider = env.ContentRootFileProvider;

    public Task<string> GetAsync(string path, CancellationToken cancellationToken)
    {
        var sqlFile = _fileProvider.GetFileInfo($"Sql/{path}");
        var sqlPath =
            sqlFile.PhysicalPath
            ?? throw new FileNotFoundException($"SQL file not found: Sql/{path}");

        return File.ReadAllTextAsync(sqlPath, cancellationToken);
    }
}
