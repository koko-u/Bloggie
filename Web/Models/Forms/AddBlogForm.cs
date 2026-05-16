using System;
using System.Collections.Generic;

namespace Bloggie.Web.Models.Forms;

/// <summary>
/// Create new blog post form
/// </summary>
public sealed class AddBlogForm
{
    /// <summary>
    /// Blog Heading title
    /// </summary>
    public string? Heading { get; set; }

    /// <summary>
    /// Blog Page title
    /// </summary>
    public string? PageTitle { get; set; }

    /// <summary>
    /// Blog Content ( Markdown format )
    /// </summary>
    public string? ContentMarkdown { get; set; }

    /// <summary>
    /// Uploaded image ids
    /// </summary>
    public List<Guid> ImageIds { get; set; } = [];

    /// <summary>
    /// Blog Short Description
    /// </summary>
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Slug for the blog post URL
    /// </summary>
    public string? Slug { get; set; }

    /// <summary>
    /// Published date of the blog post
    /// </summary>
    public DateOnly? PublishedDate { get; set; }

    /// <summary>
    /// Author of the blog post
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// the blog page is visible or not
    /// </summary>
    public bool Visible { get; set; } = false;
}
