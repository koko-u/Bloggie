using System;
using System.Collections.Generic;

namespace Bloggie.Web.Models.Domain;

/// <summary>
/// Blog Post Data Model
/// </summary>
public sealed class BlogPost
{
    /// <summary>
    /// Inner unique id ( uuid v7)
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Blog Heading title
    /// </summary>
    public required string Heading { get; set; }

    /// <summary>
    /// Blog Page title
    /// </summary>
    public required string PageTitle { get; set; }

    /// <summary>
    /// Blog Content
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Blog Short Description
    /// </summary>
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Blog Featured Image URL
    /// </summary>
    public string? FeaturedImageUrl { get; set; }

    /// <summary>
    /// Slug for the blog post URL
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// Published date of the blog post
    /// </summary>
    public DateOnly? PublishedDate { get; set; }

    /// <summary>
    /// Author of the blog post
    /// </summary>
    public required string Author { get; set; }

    /// <summary>
    /// the blog page is visible or not
    /// </summary>
    public bool Visible { get; set; } = false;

    /// <summary>
    /// Blog's Tags
    /// </summary>
    public List<Tag> Tags { get; set; } = [];
}
