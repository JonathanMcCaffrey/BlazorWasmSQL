using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _Generate.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.PostId);
                });

            migrationBuilder.InsertData(
                table: "posts",
                columns: new[] { "PostId", "Name", "PublishedOn" },
                values: new object[] { 1, "Hello World", new DateTime(2022, 3, 20, 20, 42, 27, 94, DateTimeKind.Local).AddTicks(7744) });

            migrationBuilder.InsertData(
                table: "posts",
                columns: new[] { "PostId", "Name", "PublishedOn" },
                values: new object[] { 2, "Hello World 2", new DateTime(2022, 3, 20, 20, 42, 27, 94, DateTimeKind.Local).AddTicks(7783) });

            migrationBuilder.CreateIndex(
                name: "IX_posts_PublishedOn",
                table: "posts",
                column: "PublishedOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "posts");
        }
    }
}
