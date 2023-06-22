using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWManagerServiceModelEF.Migrations
{
    /// <inheritdoc />
    public partial class userAnpassung4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataEntry_User_UserIdentityUserId",
                table: "DataEntry");

            migrationBuilder.DropIndex(
                name: "IX_DataEntry_UserIdentityUserId",
                table: "DataEntry");

            migrationBuilder.DropColumn(
                name: "UserIdentityUserId",
                table: "DataEntry");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DataEntry",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_DataEntry_UserId",
                table: "DataEntry",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataEntry_User_UserId",
                table: "DataEntry",
                column: "UserId",
                principalTable: "User",
                principalColumn: "IdentityUserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataEntry_User_UserId",
                table: "DataEntry");

            migrationBuilder.DropIndex(
                name: "IX_DataEntry_UserId",
                table: "DataEntry");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "DataEntry",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserIdentityUserId",
                table: "DataEntry",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataEntry_UserIdentityUserId",
                table: "DataEntry",
                column: "UserIdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataEntry_User_UserIdentityUserId",
                table: "DataEntry",
                column: "UserIdentityUserId",
                principalTable: "User",
                principalColumn: "IdentityUserId");
        }
    }
}
