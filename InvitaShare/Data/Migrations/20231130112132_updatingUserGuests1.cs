using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvitaShare.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatingUserGuests1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "InvitedPersons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalInvites",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvitedPersons_ApplicationUserId",
                table: "InvitedPersons",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvitedPersons_AspNetUsers_ApplicationUserId",
                table: "InvitedPersons",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvitedPersons_AspNetUsers_ApplicationUserId",
                table: "InvitedPersons");

            migrationBuilder.DropIndex(
                name: "IX_InvitedPersons_ApplicationUserId",
                table: "InvitedPersons");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "InvitedPersons");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalInvites",
                table: "AspNetUsers");
        }
    }
}
