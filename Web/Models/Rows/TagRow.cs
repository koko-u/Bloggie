using System;

namespace Bloggie.Web.Models.Rows;

public class TagRow
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid? BlogPostId { get; set; }
    public string? BlogPostHeading { get; set; }
    public string? BlogPostPageTitle { get; set; }
    public string? BlogPostContent { get; set; }
    public string? BlogPostShortDescription { get; set; }
    public Guid? BlogPostImageId { get; set; }
    public string? BlogPostImageUrl { get; set; }
    public string? BlogPostSlug { get; set; }
    public DateOnly? BlogPostPublishedDate { get; set; }
    public string? BlogPostAuthor { get; set; }
    public bool? BlogPostVisible { get; set; }
}
