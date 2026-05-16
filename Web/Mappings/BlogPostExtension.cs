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
    /// <summary>
    /// Convert query result BlogPostRow to BlogPost model ( extract blog part )
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    [MapperIgnoreSource(nameof(BlogPostRow.TagId))]
    [MapperIgnoreSource(nameof(BlogPostRow.TagName))]
    [MapperIgnoreSource(nameof(BlogPostRow.ImageId))]
    [MapperIgnoreSource(nameof(BlogPostRow.ImageUrl))]
    [MapperIgnoreTarget(nameof(BlogPost.Tags))]
    [MapperIgnoreTarget(nameof(BlogPost.Images))]
    public static partial BlogPost ToBlogPostModel(this BlogPostRow row);

    /// <summary>
    /// Convert query result BlogPostRow to Tag model ( extract tag part )
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    [MapperIgnoreSource(nameof(BlogPostRow.Id))]
    [MapperIgnoreSource(nameof(BlogPostRow.Heading))]
    [MapperIgnoreSource(nameof(BlogPostRow.PageTitle))]
    [MapperIgnoreSource(nameof(BlogPostRow.Content))]
    [MapperIgnoreSource(nameof(BlogPostRow.ShortDescription))]
    [MapperIgnoreSource(nameof(BlogPostRow.ImageId))]
    [MapperIgnoreSource(nameof(BlogPostRow.ImageUrl))]
    [MapperIgnoreSource(nameof(BlogPostRow.Slug))]
    [MapperIgnoreSource(nameof(BlogPostRow.PublishedDate))]
    [MapperIgnoreSource(nameof(BlogPostRow.Author))]
    [MapperIgnoreSource(nameof(BlogPostRow.Visible))]
    [MapperIgnoreTarget(nameof(Tag.BlogPosts))]
    [MapProperty(nameof(BlogPostRow.TagId), nameof(Tag.Id))]
    [MapProperty(nameof(BlogPostRow.TagName), nameof(Tag.Name))]
    public static partial Tag ToTagModel(this BlogPostRow row);

    [MapperIgnoreSource(nameof(BlogPostRow.Id))]
    [MapperIgnoreSource(nameof(BlogPostRow.Heading))]
    [MapperIgnoreSource(nameof(BlogPostRow.PageTitle))]
    [MapperIgnoreSource(nameof(BlogPostRow.Content))]
    [MapperIgnoreSource(nameof(BlogPostRow.ShortDescription))]
    [MapperIgnoreSource(nameof(BlogPostRow.TagId))]
    [MapperIgnoreSource(nameof(BlogPostRow.TagName))]
    [MapperIgnoreSource(nameof(BlogPostRow.Slug))]
    [MapperIgnoreSource(nameof(BlogPostRow.PublishedDate))]
    [MapperIgnoreSource(nameof(BlogPostRow.Author))]
    [MapperIgnoreSource(nameof(BlogPostRow.Visible))]
    [MapProperty(nameof(BlogPostRow.ImageId), nameof(Image.Id))]
    [MapProperty(nameof(BlogPostRow.ImageUrl), nameof(Image.Url))]
    public static partial Image ToImageModel(this BlogPostRow row);

    /// <summary>
    /// Convert BlogPost model to EditBlogForm model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [MapperIgnoreSource(nameof(BlogPost.Tags))]
    [MapperIgnoreSource(nameof(BlogPost.Images))]
    [MapperIgnoreTarget(nameof(EditBlogForm.ImageIds))]
    [MapProperty(nameof(BlogPost.Content), nameof(EditBlogForm.ContentMarkdown))]
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
                blogPost.Images = g.Where(r => r.ImageId.HasValue)
                    .Select(r => r.ToImageModel())
                    .ToList();

                return blogPost;
            });
    }
}
