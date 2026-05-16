INSERT INTO "blog_post_tags" ("blog_post_id", "tag_id")
SELECT @BlogPostId,
       "tag_id"
FROM UNNEST(@TagIds) AS "T"("tag_id")