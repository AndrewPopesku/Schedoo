using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schedoo.Server.Migrations
{
    /// <inheritdoc />
    public partial class addedNewFieldsToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_User_StudentId",
                table: "Attendances");

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "User",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Patronymic",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Attendances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_GroupId",
                table: "User",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_User_StudentId",
                table: "Attendances",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Groups_GroupId",
                table: "User",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_User_StudentId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Groups_GroupId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_GroupId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Patronymic",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SurName",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Attendances",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_User_StudentId",
                table: "Attendances",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
