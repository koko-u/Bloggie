using System;

namespace Bloggie.Web.Models.Domain;

/// <summary>
/// Images associated with blog posts
/// </summary>
public sealed class Image
{
    /// <summary>
    /// Id of the image
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Url of the image
    /// </summary>
    public required string Url { get; set; }
}
