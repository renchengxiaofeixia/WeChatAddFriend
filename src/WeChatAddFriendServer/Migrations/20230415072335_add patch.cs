using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeChatAddFriendServer.Migrations
{
    /// <inheritdoc />
    public partial class addpatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppVersions");

            migrationBuilder.CreateTable(
                name: "AppPatchs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsForceUpdate = table.Column<bool>(type: "INTEGER", nullable: false),
                    PatchVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    PatchUrl = table.Column<string>(type: "TEXT", nullable: false),
                    PatchFileName = table.Column<string>(type: "TEXT", nullable: false),
                    PatchSize = table.Column<int>(type: "INTEGER", nullable: false),
                    Tip = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPatchs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPatchs");

            migrationBuilder.CreateTable(
                name: "AppVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsForceUpdate = table.Column<bool>(type: "INTEGER", nullable: false),
                    PatchFileName = table.Column<string>(type: "TEXT", nullable: false),
                    PatchSize = table.Column<int>(type: "INTEGER", nullable: false),
                    PatchUrl = table.Column<string>(type: "TEXT", nullable: false),
                    PatchVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    Tip = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVersions", x => x.Id);
                });
        }
    }
}
