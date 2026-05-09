using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Rows;
using Riok.Mapperly.Abstractions;

namespace Bloggie.Web.Mappings;

/// <summary>
/// Query result TagRow Mappers
/// </summary>
[Mapper(AllowNullPropertyAssignment = false, ThrowOnPropertyMappingNullMismatch = true)]
public static partial class TagRowExtension
{
    /// <summary>
    /// TagRow to Tag Model Mapper
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    [MapperIgnoreSource(nameof(TagRow.BlogPostId))]
    [MapperIgnoreSource(nameof(TagRow.BlogPostHeading))]
    [MapperIgnoreSource(nameof(TagRow.BlogPostPageTitle))]
    [MapperIgnoreSource(nameof(TagRow.BlogPostContent))]
    [MapperIgnoreSource(nameof(TagRow.BlogPostShortDescription))]
    [MapperIgnoreSource(nameof(TagRow.BlogPostFeaturedImageUrl))]
    [MapperIgnoreSource(nameof(TagRow.BlogPostSlug))]
    [MapperIgnoreSource(nameof(TagRow.BlogPostPublishedDate))]
    [MapperIgnoreSource(nameof(TagRow.BlogPostAuthor))]
    [MapperIgnoreSource(nameof(TagRow.BlogPostVisible))]
    [MapperIgnoreTarget(nameof(Tag.BlogPosts))]
    public static partial Tag ToTagModel(this TagRow row);

    /// <summary>
    /// TagRow to BlogPost Model Mapper
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    [MapProperty(nameof(TagRow.BlogPostId), nameof(BlogPost.Id))]
    [MapProperty(nameof(TagRow.BlogPostHeading), nameof(BlogPost.Heading))]
    [MapProperty(nameof(TagRow.BlogPostPageTitle), nameof(BlogPost.PageTitle))]
    [MapProperty(nameof(TagRow.BlogPostContent), nameof(BlogPost.Content))]
    [MapProperty(nameof(TagRow.BlogPostShortDescription), nameof(BlogPost.ShortDescription))]
    [MapProperty(nameof(TagRow.BlogPostFeaturedImageUrl), nameof(BlogPost.FeaturedImageUrl))]
    [MapProperty(nameof(TagRow.BlogPostSlug), nameof(BlogPost.Slug))]
    [MapProperty(nameof(TagRow.BlogPostPublishedDate), nameof(BlogPost.PublishedDate))]
    [MapProperty(nameof(TagRow.BlogPostAuthor), nameof(BlogPost.Author))]
    [MapProperty(nameof(TagRow.BlogPostVisible), nameof(BlogPost.Visible))]
    [MapperIgnoreSource(nameof(TagRow.Id))]
    [MapperIgnoreSource(nameof(TagRow.Name))]
    [MapperIgnoreTarget(nameof(BlogPost.Tags))]
    public static partial BlogPost ToBlogPostModel(this TagRow row);
}
