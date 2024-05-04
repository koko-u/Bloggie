using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Db.Models.Domain;

[Table("tags")]
[Index(nameof(Name), IsUnique = true)]
public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Column("blog_post_id")]
    public Guid? BlogPostId { get; set; }

    [ForeignKey(nameof(BlogPostId))]
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public BlogPost? BlogPost { get; set; }
}
