-- ブログに紐づけられている画像テーブルに対して、更新したら本文からなくなっているので削除する
DELETE
FROM "images"
WHERE "blog_post_id" = @BlogPostId
  AND NOT ("url" = ANY (@ImageUrls))
