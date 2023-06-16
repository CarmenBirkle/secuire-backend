using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWManagerServiceModelEF.Migrations
{
    /// <inheritdoc />
    public partial class anpassungverschluesselung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentCard_CardType_CardTypeId",
                table: "PaymentCard");

            migrationBuilder.DropTable(
                name: "CardType");

            migrationBuilder.DropTable(
                name: "CustomTopic");

            migrationBuilder.DropIndex(
                name: "IX_PaymentCard_CardTypeId",
                table: "PaymentCard");

            migrationBuilder.DropColumn(
                name: "CardTypeId",
                table: "PaymentCard");

            migrationBuilder.AlterColumn<string>(
                name: "ExpirationDate",
                table: "PaymentCard",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "PaymentCard",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Favourite",
                table: "DataEntry",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "CustomTopics",
                table: "DataEntry",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SelectedIcon",
                table: "DataEntry",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardType",
                table: "PaymentCard");

            migrationBuilder.DropColumn(
                name: "CustomTopics",
                table: "DataEntry");

            migrationBuilder.DropColumn(
                name: "SelectedIcon",
                table: "DataEntry");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "PaymentCard",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CardTypeId",
                table: "PaymentCard",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "Favourite",
                table: "DataEntry",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "CardType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomTopic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataEntryId = table.Column<int>(type: "int", nullable: false),
                    FieldContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomTopic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomTopic_DataEntry_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCard_CardTypeId",
                table: "PaymentCard",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomTopic_DataEntryId",
                table: "CustomTopic",
                column: "DataEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentCard_CardType_CardTypeId",
                table: "PaymentCard",
                column: "CardTypeId",
                principalTable: "CardType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
