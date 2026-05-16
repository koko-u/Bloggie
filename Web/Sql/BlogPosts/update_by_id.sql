UPDATE "blog_posts"
SET "heading"            = @Heading,
    "page_title"         = @PageTitle,
    "content"            = @Content,
    "short_description"  = @ShortDescription,
    "slug"               = @Slug,
    "published_date"     = @PublishedDate,
    "author"             = @Author,
    "visible"            = @Visible
WHERE "id" = @Id;