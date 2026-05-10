WITH "blog_post_row" AS (SELECT "id",
                                "heading",
                                "page_title",
                                "content",
                                "short_description",
                                "featured_image_url",
                                "slug",
                                "published_date",
                                "author",
                                "visible"
                         FROM "blog_posts"
                         WHERE "slug" = @Slug)
SELECT "BP"."id",
       "BP"."heading",
       "BP"."page_title",
       "BP"."content",
       "BP"."short_description",
       "BP"."featured_image_url",
       "BP"."slug",
       "BP"."published_date",
       "BP"."author",
       "BP"."visible",
       "T"."id"   AS "tag_id",
       "T"."name" AS "tag_name"
FROM "blog_post_row" AS "BP"
         LEFT OUTER JOIN
     "blog_post_tags" AS "BPT"
     ON "BP"."id" = "BPT"."blog_post_id"
         LEFT OUTER JOIN
     "tags" AS "T"
     ON "BPT"."tag_id" = "T"."id"
