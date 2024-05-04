using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloggie.Db.Models.Domain;

[Table("blog_posts")]
public class BlogPost
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("heading")]
    [MaxLength(255)]
    public string Heading { get; set; } = string.Empty;

    [Column("page_title")]
    [MaxLength(255)]
    public string PageTitle { get; set; } = string.Empty;

    [Column("content")]
    [MaxLength(int.MaxValue)]
    public string Content { get; set; } = string.Empty;

    [Column("short_description")]
    [MaxLength(255)]
    public string ShortDescription { get; set; } = string.Empty;

    [Column("featured_image_url")]
    [MaxLength(255)]
    public string FeaturedImageUrl { get; set; } = string.Empty;

    [Column("url_handle")]
    [MaxLength(255)]
    public string UrlHandle { get; set; } = string.Empty;

    [Column("published_on")]
    public DateTime? PublishedOn { get; set; }

    [Column("author")]
    [MaxLength(255)]
    public string Author { get; set; } = string.Empty;

    [Column("is_visible")]
    [DefaultValue(false)]
    public bool IsVisible { get; set; } = false;

}
