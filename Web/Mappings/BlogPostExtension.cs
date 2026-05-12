using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Rows;
using Riok.Mapperly.Abstractions;

namespace Bloggie.Web.Mappings;

[Mapper(ThrowOnPropertyMappingNullMismatch = true)]
public static partial class BlogPostExtension
{
    [MapperIgnoreSource(nameof(BlogPostRow.TagId))]
    [MapperIgnoreSource(nameof(BlogPostRow.TagName))]
    [MapperIgnoreTarget(nameof(BlogPost.Tags))]
    public static partial BlogPost ToBlogPostModel(this BlogPostRow row);

    [MapperIgnoreSource(nameof(BlogPostRow.Id))]
    [MapperIgnoreSource(nameof(BlogPostRow.Heading))]
    [MapperIgnoreSource(nameof(BlogPostRow.PageTitle))]
    [MapperIgnoreSource(nameof(BlogPostRow.Content))]
    [MapperIgnoreSource(nameof(BlogPostRow.ShortDescription))]
    [MapperIgnoreSource(nameof(BlogPostRow.FeaturedImageUrl))]
    [MapperIgnoreSource(nameof(BlogPostRow.Slug))]
    [MapperIgnoreSource(nameof(BlogPostRow.PublishedDate))]
    [MapperIgnoreSource(nameof(BlogPostRow.Author))]
    [MapperIgnoreSource(nameof(BlogPostRow.Visible))]
    [MapperIgnoreTarget(nameof(Tag.BlogPosts))]
    [MapProperty(nameof(BlogPostRow.TagId), nameof(Tag.Id))]
    [MapProperty(nameof(BlogPostRow.TagName), nameof(Tag.Name))]
    public static partial Tag ToTagModel(this BlogPostRow row);
}
