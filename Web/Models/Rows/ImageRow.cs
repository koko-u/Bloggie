using System;

namespace Bloggie.Web.Models.Rows;

public sealed class ImageRow
{
    public required Guid Id { get; set; }

    public Guid? BlogPostId { get; set; }

    public required string Url { get; set; }
}
