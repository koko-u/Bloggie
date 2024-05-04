using Bloggie.Db.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Db.Data;

public class BloggieDbContext(DbContextOptions<BloggieDbContext> opt) : DbContext(opt)
{
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPost>()
            .Property(b => b.Id)
            .HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<BlogPost>()
            .Property(b => b.IsVisible)
            .HasDefaultValue(false);

        modelBuilder.Entity<Tag>()
            .Property(t => t.Id)
            .HasDefaultValueSql("NEWID()");
    }
}
