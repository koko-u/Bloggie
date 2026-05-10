using System;
using System.ComponentModel;

namespace Bloggie.Web.Models.Forms;

/// <summary>
/// Create new blog post form
/// </summary>
public sealed class AddBlogForm
{
    /// <summary>
    /// Blog Heading title
    /// </summary>
    [DisplayName("Heading")]
    public string? Heading { get; set; }

    /// <summary>
    /// Blog Page title
    /// </summary>
    [DisplayName("Page Title")]
    public string? PageTitle { get; set; }

    /// <summary>
    /// Blog Content
    /// </summary>
    [DisplayName("Content")]
    public string? Content { get; set; }

    /// <summary>
    /// Blog Short Description
    /// </summary>
    [DisplayName("Short Description")]
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Blog Featured Image URL
    /// </summary>
    [DisplayName("Featured Image")]
    public string? FeaturedImageUrl { get; set; }

    /// <summary>
    /// Slug for the blog post URL
    /// </summary>
    [DisplayName("Slug")]
    public string? Slug { get; set; }

    /// <summary>
    /// Published date of the blog post
    /// </summary>
    [DisplayName("Published Date")]
    public DateOnly? PublishedDate { get; set; }

    /// <summary>
    /// Author of the blog post
    /// </summary>
    [DisplayName("Author")]
    public string? Author { get; set; }

    /// <summary>
    /// the blog page is visible or not
    /// </summary>
    [DisplayName("Is Visible")]
    public bool Visible { get; set; } = false;
}
