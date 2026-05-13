INSERT INTO "blog_posts" ("heading",
                          "page_title",
                          "content",
                          "short_description",
                          "featured_image_url",
                          "slug",
                          "published_date",
                          "author",
                          "visible")
VALUES (@Heading,
        @PageTitle,
        @Content,
        @ShortDescription,
        @FeaturedImageUrl,
        @Slug,
        @PublishedDate,
        @Author,
        @Visible)
RETURNING "id";