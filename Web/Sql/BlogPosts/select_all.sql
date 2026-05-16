SELECT "BP"."id",
       "BP"."heading",
       "BP"."page_title",
       "BP"."content",
       "BP"."short_description",
       "I"."id"   AS "image_id",
       "I"."url"  AS "image_url",
       "BP"."slug",
       "BP"."published_date",
       "BP"."author",
       "BP"."visible",
       "T"."id"   AS "tag_id",
       "T"."name" AS "tag_name"
FROM "blog_posts" AS "BP"
         LEFT OUTER JOIN
     "blog_post_tags" AS "BPT"
     ON "BP"."id" = "BPT"."blog_post_id"
         LEFT OUTER JOIN
     "tags" AS "T"
     ON "BPT"."tag_id" = "T"."id"
         LEFT OUTER JOIN
     "images" AS "I"
     ON "I"."blog_post_id" = "BP"."id"
ORDER BY "BP"."published_date" DESC,
         "BP"."id";
