-- migrate:up
CREATE TABLE IF NOT EXISTS "blog_posts"
(
    "id"                 UUID          NOT NULL DEFAULT "uuidv7"(),
    "page_title"         VARCHAR(255)  NOT NULL,
    "content"            TEXT          NULL     DEFAULT NULL,
    "short_description"  VARCHAR(255)  NULL     DEFAULT NULL,
    "featured_image_url" VARCHAR(1024) NULL     DEFAULT NULL,
    "slug"               VARCHAR(255)  NOT NULL,
    "published_date"     DATE          NULL     DEFAULT NULL,
    "author"             VARCHAR(255)  NOT NULL,
    "visible"            BOOLEAN       NOT NULL DEFAULT FALSE,
    "created_at"         TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    "updated_at"         TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    CONSTRAINT "blog_posts_pkey" PRIMARY KEY ("id")
);

CREATE INDEX IF NOT EXISTS "idx_blog_posts_title" ON "blog_posts" ("page_title");
CREATE INDEX IF NOT EXISTS "idx_blog_posts_title_like" ON "blog_posts" USING "gin" ("page_title" "gin_trgm_ops");
CREATE INDEX IF NOT EXISTS "idx_blog_posts_visible" ON "blog_posts" ("visible");
CREATE UNIQUE INDEX IF NOT EXISTS "uq_blog_posts_slug" ON "blog_posts" ("slug");

CREATE OR REPLACE TRIGGER "blog_posts_updated_at"
    BEFORE UPDATE 
    ON "blog_posts"
    FOR EACH ROW
    EXECUTE PROCEDURE moddatetime("updated_at");

-- migrate:down
DROP TABLE IF EXISTS "blog_posts";
