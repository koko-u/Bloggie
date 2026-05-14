DELETE
FROM "blog_posts"
WHERE "id" = @Id
RETURNING "id",
    "heading",
    "page_title",
    "content",
    "short_description",
    "featured_image_url",
    "slug",
    "published_date",
    "author",
    "visible";