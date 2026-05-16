-- ブログに紐づけられていない画像テーブルに対して、ブログ登録時に本文から消えているので削除する
DELETE FROM "images"
WHERE "id" = ANY (@ImageIds)
  AND "url" NOT IN (@ImageUrls)
  AND "blog_post_id" IS NULL;  