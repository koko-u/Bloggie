using System;

namespace Bloggie.Web.Models.Rows;

public sealed class BlogPostRow
{
    public required Guid Id { get; set; }

    public required string Heading { get; set; }

    public required string PageTitle { get; set; }

    public string? Content { get; set; }

    public string? ShortDescription { get; set; }

    public Guid? ImageId { get; set; }

    public string? ImageUrl { get; set; }

    public required string Slug { get; set; }

    public DateOnly? PublishedDate { get; set; }

    public required string Author { get; set; }

    public required bool Visible { get; set; } = false;

    public Guid? TagId { get; set; }

    public string? TagName { get; set; }
}
