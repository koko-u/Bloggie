DELETE
FROM "blog_posts"
WHERE "id" = @Id
RETURNING "id",
    "heading",
    "page_title",
    "content",
    "short_description",
    "slug",
    "published_date",
    "author",
    "visible";