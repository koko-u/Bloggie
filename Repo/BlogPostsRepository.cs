using AutoMapper;
using Bloggie.Db.Data;
using Bloggie.Db.Models.Domain;
using Bloggie.Repo.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using RustyOptions;

namespace Bloggie.Repo;

public class BlogPostsRepository(BloggieDbContext dbContext, IMapper mapper) : IBlogPostsRepository
{
    public async Task<IEnumerable<BlogPostRow>> GetAllAsync()
    {
        var blogPosts = await dbContext.BlogPosts.ToListAsync();
        return mapper.Map<List<BlogPost>, List<BlogPostRow>>(blogPosts);
    }

    public async Task<Result<BlogPostRow, ErrorMessage>> GetByIdAsync(Guid id)
    {
        var blogPost = await dbContext.BlogPosts.SingleOrDefaultAsync(blogPost => blogPost.Id == id);
        if (blogPost is null)
        {
            return Result.Err<BlogPostRow, ErrorMessage>(new ErrorMessage($"Id {id} is not found."));
        }

        return Result.Ok<BlogPostRow, ErrorMessage>(mapper.Map<BlogPost, BlogPostRow>(blogPost));
    }

    public async Task<BlogPostRow> CreateAsync(AddBlogPost addBlogPost)
    {
        var blogPost = new BlogPost();
        mapper.Map(addBlogPost, blogPost);

        await dbContext.BlogPosts.AddAsync(blogPost);
        await dbContext.SaveChangesAsync();

        return mapper.Map<BlogPost, BlogPostRow>(blogPost);
    }

    public async Task<Result<BlogPostRow, ErrorMessage>> UpdateAsync(Guid id, BlogPostRow updateBlogPost)
    {
        var blogPost = await dbContext.BlogPosts.SingleOrDefaultAsync(blogPost => blogPost.Id == id);
        if (blogPost is null)
        {
            return Result.Err<BlogPostRow, ErrorMessage>(new ErrorMessage($"Id {id} is not found."));
        }

        mapper.Map(updateBlogPost, blogPost);
        await dbContext.SaveChangesAsync();

        return Result.Ok<BlogPostRow, ErrorMessage>(mapper.Map<BlogPost, BlogPostRow>(blogPost));
    }

    public async Task<Result<BlogPostRow, ErrorMessage>> DeleteByIdAsync(Guid id)
    {
        var blogPost = await dbContext.BlogPosts.SingleOrDefaultAsync(blogPost => blogPost.Id == id);
        if (blogPost is null)
        {
            return Result.Err<BlogPostRow, ErrorMessage>(new ErrorMessage($"Id {id} is not found."));
        }

        dbContext.BlogPosts.Remove(blogPost);
        await dbContext.SaveChangesAsync();

        return Result.Ok<BlogPostRow, ErrorMessage>(mapper.Map<BlogPost, BlogPostRow>(blogPost));
    }
}
