UPDATE "images"
SET "blog_post_id" = @BlogPostId
WHERE "id" = ANY (@ImageIds)
  AND "url" = ANY (@ImageUrls)
  AND "blog_post_id" IS NULL; 