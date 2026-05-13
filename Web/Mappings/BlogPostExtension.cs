using System.Collections.Generic;
using System.Linq;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Forms;
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

    [MapperIgnoreSource(nameof(BlogPost.Tags))]
    public static partial EditBlogForm ToEditBlogForm(this BlogPost model);

    /// <summary>
    /// Grouping blog_posts query result into BlogPost list
    /// </summary>
    /// <param name="rows"></param>
    /// <returns></returns>
    public static IEnumerable<BlogPost> GroupByBlogPost(this IEnumerable<BlogPostRow> rows)
    {
        return rows.GroupBy(r => r.ToBlogPostModel())
            .Select(g =>
            {
                var blogPost = g.Key.FastDeepClone();
                blogPost.Tags = g.Where(r => r.TagId.HasValue).Select(r => r.ToTagModel()).ToList();

                return blogPost;
            });
    }
}
