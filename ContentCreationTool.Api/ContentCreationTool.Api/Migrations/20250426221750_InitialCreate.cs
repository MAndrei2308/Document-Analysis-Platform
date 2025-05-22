using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentCreationTool.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    ModelTypeUsed = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    ExtractedText = table.Column<string>(type: "TEXT", nullable: true),
                    Summary = table.Column<string>(type: "TEXT", nullable: true),
                    ContentItemId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageDocuments_ContentItems_ContentItemId",
                        column: x => x.ContentItemId,
                        principalTable: "ContentItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    ExtractedText = table.Column<string>(type: "TEXT", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", nullable: true),
                    ContentItemId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextDocuments_ContentItems_ContentItemId",
                        column: x => x.ContentItemId,
                        principalTable: "ContentItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageDocuments_ContentItemId",
                table: "ImageDocuments",
                column: "ContentItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TextDocuments_ContentItemId",
                table: "TextDocuments",
                column: "ContentItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageDocuments");

            migrationBuilder.DropTable(
                name: "TextDocuments");

            migrationBuilder.DropTable(
                name: "ContentItems");
        }
    }
}
