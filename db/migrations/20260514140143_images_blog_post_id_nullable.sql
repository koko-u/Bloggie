-- migrate:up
ALTER TABLE IF EXISTS "images"
    ALTER COLUMN "blog_post_id" DROP NOT NULL;
ALTER TABLE IF EXISTS "images"
    DROP CONSTRAINT IF EXISTS "images_blog_post_id_fkey";
ALTER TABLE IF EXISTS "images"
    ADD CONSTRAINT "images_blog_post_id_fkey" FOREIGN KEY ("blog_post_id") REFERENCES "blog_posts" ("id") ON DELETE SET NULL;

-- migrate:down
ALTER TABLE IF EXISTS "images"
    ALTER COLUMN "blog_post_id" SET NOT NULL;
ALTER TABLE IF EXISTS "images"
    DROP CONSTRAINT IF EXISTS "images_blog_post_id_fkey";
ALTER TABLE IF EXISTS "images"
    ADD CONSTRAINT "images_blog_post_id_fkey" FOREIGN KEY ("blog_post_id") REFERENCES "blog_posts" ("id") ON DELETE CASCADE;
