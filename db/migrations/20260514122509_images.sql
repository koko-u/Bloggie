-- migrate:up
CREATE TABLE IF NOT EXISTS "images" (
    "id" UUID NOT NULL DEFAULT uuidv7(),
    "blog_post_id" UUID NOT NULL,
    "url" TEXT NOT NULL,
    "created_at" TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT "images_pkey" PRIMARY KEY ("id"),
    CONSTRAINT "images_blog_post_id_fkey" FOREIGN KEY ("blog_post_id") REFERENCES "blog_posts"("id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "idx_images_blog_post_id" ON "images" ("blog_post_id");


-- migrate:down
DROP TABLE IF EXISTS "images";
