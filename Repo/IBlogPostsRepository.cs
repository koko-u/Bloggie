using Bloggie.Repo.Models.ViewModels;

namespace Bloggie.Repo;

public interface IBlogPostsRepository
{
    Task<IEnumerable<BlogPostRow>> GetAllAsync();
    Task<BlogPostRow?> GetByIdAsync(Guid id);
    Task<BlogPostRow> CreateAsync(AddBlogPost addBlogPost);
    Task<BlogPostRow?> UpdateAsync(Guid id, BlogPostRow updateBlogPost);
    Task<BlogPostRow?> DeleteByIdAsync(Guid id);
}
