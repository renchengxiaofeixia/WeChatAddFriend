using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeChatAddFriendServer.Migrations
{
    /// <inheritdoc />
    public partial class addwechatphone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "AppPatchs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "AppPatchs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WeChatPhones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhoneNo = table.Column<string>(type: "TEXT", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeChatPhones", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeChatPhones");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "AppPatchs");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "AppPatchs");
        }
    }
}
