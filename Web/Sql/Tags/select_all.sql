SELECT "T"."id",
       "T"."name",
       "BP"."id"                 AS "blog_post_id",
       "BP"."heading"            AS "blog_post_heading",
       "BP"."page_title"         AS "blog_post_title",
       "BP"."content"            AS "blog_post_content",
       "BP"."short_description"  AS "blog_post_short_description",
       "BP"."featured_image_url" AS "blog_post_featured_image_url",
       "BP"."slug"               AS "blog_post_slug",
       "BP"."published_date"     AS "blog_post_published_date",
       "BP"."author"             AS "blog_post_author",
       "BP"."visible"            AS "blog_post_visible"
FROM "tags" AS "T"
         LEFT OUTER JOIN
     "blog_post_tags" AS "BPT"
     ON "T"."id" = "BPT"."tag_id"
         LEFT OUTER JOIN
     "blog_posts" AS "BP"
     ON "BPT"."blog_post_id" = "BP"."id";