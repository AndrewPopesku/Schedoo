using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schedoo.Server.Migrations.SchedooIdentity
{
    /// <inheritdoc />
    public partial class addedNewFieldsToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Patronymic",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6cc6af6a-edc3-4715-9b09-f7638543fe68",
                columns: new[] { "ConcurrencyStamp", "GroupId", "Name", "PasswordHash", "Patronymic", "Position", "SecurityStamp", "SurName" },
                values: new object[] { "2c7dbd64-b01c-43c7-97c6-726944b1e0a4", null, "Vasya", "AQAAAAIAAYagAAAAEP7KXlb9gEpv8BjBXZV+I/u2VyLJFgGYGzPW4ph8Wj7pOX00zie4zeqHHefYFNHZNg==", "Georgiyovich", null, "c682b7ce-7b5c-4781-a136-a58716a4e4aa", "Hrosu" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98020454-c731-45e3-980c-1d1cec0ba390",
                columns: new[] { "ConcurrencyStamp", "GroupId", "Name", "PasswordHash", "Patronymic", "Position", "SecurityStamp", "SurName" },
                values: new object[] { "b6762249-ac05-4b3d-949e-339d604d539c", null, "Misha", "AQAAAAIAAYagAAAAEF+qeRSnfgO677XQRTt1d3Q5wkxuKni6Tr99oYmqhq9nCwDqjV5FIi9dMuCCXeHcrw==", "Yuriyovich", null, "78e4edb7-5e10-42bb-ba19-2200cb442678", "Paranchich" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9f4a48ff-1244-49ef-80ea-c70eb70d27e0",
                columns: new[] { "ConcurrencyStamp", "GroupId", "Name", "PasswordHash", "Patronymic", "Position", "SecurityStamp", "SurName" },
                values: new object[] { "21199af1-bbcf-4d63-8a4d-9d386a379131", null, "Admin", "AQAAAAIAAYagAAAAELCUw8A6BHPqqmFmBR+3VLIMvYIR5SwwHI0KYHK2wpugokLYmb/uTzz0Uu4Jjxv7dg==", "Admin", null, "c60efa2a-046d-4eee-a7f0-b6bc75061d2c", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Group_GroupId",
                table: "AspNetUsers",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Group_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Patronymic",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SurName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6cc6af6a-edc3-4715-9b09-f7638543fe68",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1625a363-f103-4ca6-b7d9-f1e56f7f81c6", "AQAAAAIAAYagAAAAEDORlMQKOPTO+o0WM54Jpy3t3f1mOMesnhNBoHuuOFKQYh/iIDSE6llI7PSSAfXDsw==", "e1280bd9-bcec-4213-852f-9413c984b90d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98020454-c731-45e3-980c-1d1cec0ba390",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4d1ee76e-de16-4de5-aa5d-f7a6264bcb04", "AQAAAAIAAYagAAAAEA5M8qMXkPdsezgEZyRXNR0E586MploIOHHOzXASDhc7AiShtdmHaM4k4AzapNOA0Q==", "8073bd9b-6196-411b-858e-f8dd04f7d7fa" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9f4a48ff-1244-49ef-80ea-c70eb70d27e0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4a56bd0a-6d0a-4da9-9b5e-85680babf525", "AQAAAAIAAYagAAAAEFs3xIII4TLSeOhg6e+r7NRIw3jUBTydJesrbOaZcMSfrhvs+Yb/1gsCB+ZX1xC/Tw==", "e30551f1-373f-4af6-bf9c-ee8cc369d615" });
        }
    }
}
