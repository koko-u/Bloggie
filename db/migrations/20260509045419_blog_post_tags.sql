-- migrate:up
CREATE TABLE IF NOT EXISTS "blog_post_tags" (
    "blog_post_id" UUID NOT NULL,
    "tag_id" UUID NOT NULL,
    CONSTRAINT "blog_post_tags_pkey" PRIMARY KEY ("blog_post_id", "tag_id"),
    CONSTRAINT "blog_post_tags_blog_post" FOREIGN KEY ("blog_post_id") REFERENCES "blog_posts"("id"),
    CONSTRAINT "blog_post_tags_tag" FOREIGN KEY ("tag_id") REFERENCES "tags"("id")
);

CREATE INDEX IF NOT EXISTS "idx_blog_post_tags_blog_post_id" ON "blog_post_tags"("blog_post_id");
CREATE INDEX IF NOT EXISTS "idx_blog_post_tags_tag_id" ON "blog_post_tags"("tag_id");

-- migrate:down
DROP TABLE IF EXISTS "blog_post_tags";

