using AutoMapper;
using Bloggie.Db.Data;
using Bloggie.Db.Models.Domain;
using Bloggie.Repo.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Repo;

public class BlogPostsRepository(BloggieDbContext dbContext, IMapper mapper) : IBlogPostsRepository
{
    public async Task<IEnumerable<BlogPostRow>> GetAllAsync()
    {
        var blogPosts = await dbContext.BlogPosts.ToListAsync();
        return mapper.Map<List<BlogPost>, List<BlogPostRow>>(blogPosts);
    }

    public async Task<BlogPostRow?> GetByIdAsync(Guid id)
    {
        var blogPost = await dbContext.BlogPosts.SingleOrDefaultAsync(blogPost => blogPost.Id == id);
        if (blogPost is null)
        {
            return null;
        }

        return mapper.Map<BlogPost, BlogPostRow>(blogPost);
    }

    public async Task<BlogPostRow> CreateAsync(AddBlogPost addBlogPost)
    {
        var blogPost = new BlogPost();
        mapper.Map(addBlogPost, blogPost);

        await dbContext.BlogPosts.AddAsync(blogPost);
        await dbContext.SaveChangesAsync();

        return mapper.Map<BlogPost, BlogPostRow>(blogPost);
    }

    public async Task<BlogPostRow?> UpdateAsync(Guid id, BlogPostRow updateBlogPost)
    {
        var blogPost = await dbContext.BlogPosts.SingleOrDefaultAsync(blogPost => blogPost.Id == id);
        if (blogPost is null)
        {
            return null;
        }

        mapper.Map(updateBlogPost, blogPost);
        await dbContext.SaveChangesAsync();

        return mapper.Map<BlogPost, BlogPostRow>(blogPost);
    }

    public async Task<BlogPostRow?> DeleteByIdAsync(Guid id)
    {
        var blogPost = await dbContext.BlogPosts.SingleOrDefaultAsync(blogPost => blogPost.Id == id);
        if (blogPost is null)
        {
            return null;
        }

        dbContext.BlogPosts.Remove(blogPost);
        await dbContext.SaveChangesAsync();

        return mapper.Map<BlogPost, BlogPostRow>(blogPost);
    }
}
