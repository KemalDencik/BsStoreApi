using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WepApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0fb8a714-a809-448f-a64a-2490c4b6cf7a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d6f46b3-5b87-45db-835b-811cc834a52e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd7be7f3-92f3-46aa-8cb3-feb392463aef");

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "42a4efe2-b777-496e-a82c-c585f1d29fbe", "bf911987-62eb-43d1-bb71-65ef0525eef8", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5f205351-e786-44be-af50-b19ed309837d", "47e955f4-064d-459a-88f7-10d5915d39e1", "editor", "EDITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cc30476e-d94f-4d0e-81bd-de5d56590b58", "ccdfefdb-ada3-46e8-aca6-e0317fa10df5", "user", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadedFiles");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42a4efe2-b777-496e-a82c-c585f1d29fbe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f205351-e786-44be-af50-b19ed309837d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc30476e-d94f-4d0e-81bd-de5d56590b58");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0fb8a714-a809-448f-a64a-2490c4b6cf7a", "e85e93b4-e8cd-4a87-b193-b909db0fe579", "editor", "EDITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3d6f46b3-5b87-45db-835b-811cc834a52e", "ad8a5c22-069d-44e0-9ca1-581c5dc41d52", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dd7be7f3-92f3-46aa-8cb3-feb392463aef", "38fbac45-c4f5-4db8-8cf0-e70e0c33dfe3", "user", "USER" });
        }
    }
}
