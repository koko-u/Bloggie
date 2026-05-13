UPDATE "blog_posts"
SET "heading"            = @Heading,
    "page_title"         = @PageTitle,
    "content"            = @Content,
    "short_description"  = @ShortDescription,
    "featured_image_url" = @FeaturedImageUrl,
    "slug"               = @Slug,
    "published_date"     = @PublishedDate,
    "author"             = @Author,
    "visible"            = @Visible
WHERE "id" = @Id;