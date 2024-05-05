namespace Bloggie.Web.Models.ViewModels;

public class BlogPostRow
{
    public required Guid Id { get; set; }

    public required string Heading { get; set; }

    public required string PageTitle { get; set; }

    public required string Content { get; set; }

    public required string ShortDescription { get; set; }

    public required string FeaturedImageUrl { get; set; }

    public required string UrlHandle { get; set; }

    public DateTime? PublishedOn { get; set; }

    public required string Author { get; set; }

    public bool IsVisible { get; set; } = false;
}
