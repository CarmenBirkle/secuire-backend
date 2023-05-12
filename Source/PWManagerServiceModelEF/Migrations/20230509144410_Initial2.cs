using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWManagerServiceModelEF.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DataEntries",
                table: "DataEntries");

            migrationBuilder.RenameTable(
                name: "DataEntries",
                newName: "DataEntry");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "DataEntry",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "DataEntry",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "DataEntry",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "DataEntry",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataEntry",
                table: "DataEntry",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DataEntry",
                table: "DataEntry");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "DataEntry");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "DataEntry");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "DataEntry");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "DataEntry");

            migrationBuilder.RenameTable(
                name: "DataEntry",
                newName: "DataEntries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataEntries",
                table: "DataEntries",
                column: "Id");
        }
    }
}
