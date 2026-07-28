using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class FinalLibraryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Librarians",
                columns: table => new
                {
                    LibrarianId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Librarians", x => x.LibrarianId);
                });

            migrationBuilder.CreateTable(
                name: "LoginModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.InsertData(
                table: "Librarians",
                columns: new[] { "LibrarianId", "Age", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, 34, "Sarah Connor", "555-0201" },
                    { 2, 28, "John Doe", "555-0202" },
                    { 3, 45, "Michael Scott", "555-0203" }
                });

            migrationBuilder.InsertData(
                table: "LoginModels",
                columns: new[] { "Id", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "12345", "admin" },
                    { 2, "myc546", "mycodingproject" },
                    { 3, "myc", "my" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Email", "Phone", "StudentName" },
                values: new object[,]
                {
                    { 1, "alice@email.com", "555-0101", "Alice Johnson" },
                    { 2, "bob@email.com", "555-0102", "Bob Smith" },
                    { 3, "charlie@email.com", "555-0103", "Charlie Brown" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Librarians");

            migrationBuilder.DropTable(
                name: "LoginModels");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
