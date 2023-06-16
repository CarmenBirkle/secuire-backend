using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWManagerServiceModelEF.Migrations
{
    /// <inheritdoc />
    public partial class userAnpassung2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_AspNetUsers_IdentUserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_IdentUserId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IdentUserId",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentUserId",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdentUserId",
                table: "User",
                column: "IdentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_AspNetUsers_IdentUserId",
                table: "User",
                column: "IdentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
