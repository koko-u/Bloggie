-- migrate:up
CREATE TABLE IF NOT EXISTS "tags" (
    "id" UUID NOT NULL DEFAULT uuidv7(),
    "name" VARCHAR(255) NOT NULL,
    CONSTRAINT "tags_pkey" PRIMARY KEY ("id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "uq_tags_name" ON "tags" ("name");

-- migrate:down
DROP TABLE IF EXISTS "tags";
