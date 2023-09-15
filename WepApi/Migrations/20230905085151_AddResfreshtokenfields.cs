using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WepApi.Migrations
{
    public partial class AddResfreshtokenfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c18518a-44a8-486a-ae1d-d8f2be895b29");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c21afa0e-9643-4504-94d0-ddce2f16de59");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5099db5-5dff-41ce-a84f-ae80053d2223");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9c18518a-44a8-486a-ae1d-d8f2be895b29", "6dde73d6-23ac-48b8-913d-6c3f519111dd", "user", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c21afa0e-9643-4504-94d0-ddce2f16de59", "2a09609c-a131-44fa-841a-87c75f3231f5", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c5099db5-5dff-41ce-a84f-ae80053d2223", "2218d915-dbea-461d-ba0d-c28163a5b6ac", "editor", "EDITOR" });
        }
    }
}
