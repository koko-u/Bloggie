using System.Collections.Generic;
using System.Linq;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Bloggie.Web.Utils;

public static class MarkdownImageUrlExtractor
{
    /// <summary>
    /// Extract image urls from markdown content
    /// </summary>
    /// <param name="markdownContent"></param>
    /// <returns>url string list in the markdown content</returns>
    public static IEnumerable<string> ExtractImageUrls(this string markdownContent)
    {
        var pipeline = new MarkdownPipelineBuilder().Build();
        var markdownDocument = Markdown.Parse(markdownContent, pipeline);

        return markdownDocument
            .Descendants<LinkInline>()
            .Where(link => link is { IsImage: true, Url: not null })
            .Select(link => link.Url!);
    }
}
