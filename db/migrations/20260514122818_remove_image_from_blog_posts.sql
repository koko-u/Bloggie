-- migrate:up
ALTER TABLE IF EXISTS "blog_posts"
DROP COLUMN IF EXISTS "featured_image_url";

-- migrate:down
ALTER TABLE IF EXISTS "blog_posts"
ADD COLUMN IF NOT EXISTS "featured_image_url" VARCHAR(1024);

