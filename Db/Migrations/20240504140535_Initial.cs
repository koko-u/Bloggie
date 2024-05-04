using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Db.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blog_posts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    heading = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    page_title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    short_description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    featured_image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    url_handle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    published_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    author = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    is_visible = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_posts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    blog_post_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_tags_blog_posts_blog_post_id",
                        column: x => x.blog_post_id,
                        principalTable: "blog_posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tags_blog_post_id",
                table: "tags",
                column: "blog_post_id");

            migrationBuilder.CreateIndex(
                name: "IX_tags_name",
                table: "tags",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "blog_posts");
        }
    }
}
