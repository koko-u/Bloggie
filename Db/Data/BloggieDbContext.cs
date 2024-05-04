using Bloggie.Db.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Db.Data;

public class BloggieDbContext(DbContextOptions<BloggieDbContext> opt) : DbContext(opt)
{
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<Tag> Tags => Set<Tag>();
}
