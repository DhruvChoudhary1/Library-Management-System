using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddMagazineAndNewspaper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Magazines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Publisher = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magazines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Newspapers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Publisher = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newspapers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Magazines",
                columns: new[] { "Id", "Category", "IsAvailable", "IssueDate", "Publisher", "Title" },
                values: new object[,]
                {
                    { 1, "Science", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "National Geographic Society", "National Geographic" },
                    { 2, "News", true, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Time USA LLC", "TIME" },
                    { 3, "Business", true, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Forbes Media", "Forbes" }
                });

            migrationBuilder.InsertData(
                table: "Newspapers",
                columns: new[] { "Id", "IsAvailable", "PublicationDate", "Publisher", "Title" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bennett Coleman", "The Times of India" },
                    { 2, true, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Hindu Group", "The Hindu" },
                    { 3, true, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Express Group", "Indian Express" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Magazines");

            migrationBuilder.DropTable(
                name: "Newspapers");
        }
    }
}
