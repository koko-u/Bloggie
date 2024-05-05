using Bloggie.Repo.Models.ViewModels;
using RustyOptions;

namespace Bloggie.Repo;

public interface IBlogPostsRepository
{
    Task<IEnumerable<BlogPostRow>> GetAllAsync();
    Task<Result<BlogPostRow, ErrorMessage>> GetByIdAsync(Guid id);
    Task<BlogPostRow> CreateAsync(AddBlogPost addBlogPost);
    Task<Result<BlogPostRow, ErrorMessage>> UpdateAsync(Guid id, BlogPostRow updateBlogPost);
    Task<Result<BlogPostRow, ErrorMessage>> DeleteByIdAsync(Guid id);
}
