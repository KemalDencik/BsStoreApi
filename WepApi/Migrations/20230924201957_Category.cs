using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WepApi.Migrations
{
    public partial class Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2853154b-c4a0-40dc-a61b-4bf9818d05f4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c54efa6-7836-40a7-8b94-f15c8c407310");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9f2dc94-d722-4e62-a6cb-7cf413e5cf30");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5529f58c-8a8f-4fa1-a3b4-31fb2b8b5b25", "f0c0ca56-fe0a-4a42-b1eb-e44356aa23b8", "admin", "ADMIN" },
                    { "82bf2d95-94e7-4bd7-9e04-a3101f08ed20", "0ac79ee8-03d5-4d51-a080-315699950b12", "user", "USER" },
                    { "c457c6a1-b77f-4477-8f35-696c010e1b87", "0f860ae6-6622-4eb9-a1e0-ef8f9b6282ca", "editor", "EDITOR" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Computer Science" },
                    { 2, "Sql Science" },
                    { 3, "API Science" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5529f58c-8a8f-4fa1-a3b4-31fb2b8b5b25");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82bf2d95-94e7-4bd7-9e04-a3101f08ed20");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c457c6a1-b77f-4477-8f35-696c010e1b87");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2853154b-c4a0-40dc-a61b-4bf9818d05f4", "9f97f5ce-e633-408b-b7dc-bebda77ec26d", "user", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4c54efa6-7836-40a7-8b94-f15c8c407310", "d1a1f7bf-79e2-40c6-a297-87914b648e08", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e9f2dc94-d722-4e62-a6cb-7cf413e5cf30", "3281b2ff-dbfb-4b15-9802-7d3d77eb8edc", "editor", "EDITOR" });
        }
    }
}
