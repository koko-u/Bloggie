INSERT INTO "blog_posts" ("heading",
                          "page_title",
                          "content",
                          "short_description",
                          "slug",
                          "published_date",
                          "author",
                          "visible")
VALUES (@Heading,
        @PageTitle,
        @Content,
        @ShortDescription,
        @Slug,
        @PublishedDate,
        @Author,
        @Visible)
RETURNING "id";