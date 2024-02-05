using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvitaShare.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreateEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PartnerName2",
                table: "Events",
                newName: "WeddingEvent_GodParent2");

            migrationBuilder.RenameColumn(
                name: "PartnerName1",
                table: "Events",
                newName: "WeddingEvent_GodParent1");

            migrationBuilder.RenameColumn(
                name: "ParentName2",
                table: "Events",
                newName: "MotherName");

            migrationBuilder.RenameColumn(
                name: "ParentName1",
                table: "Events",
                newName: "GodParent2");

            migrationBuilder.RenameColumn(
                name: "ParentName",
                table: "Events",
                newName: "GodParent1");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Events",
                newName: "EventName");

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "WeddingEvent_GodParent2",
                table: "Events",
                newName: "PartnerName2");

            migrationBuilder.RenameColumn(
                name: "WeddingEvent_GodParent1",
                table: "Events",
                newName: "PartnerName1");

            migrationBuilder.RenameColumn(
                name: "MotherName",
                table: "Events",
                newName: "ParentName2");

            migrationBuilder.RenameColumn(
                name: "GodParent2",
                table: "Events",
                newName: "ParentName1");

            migrationBuilder.RenameColumn(
                name: "GodParent1",
                table: "Events",
                newName: "ParentName");

            migrationBuilder.RenameColumn(
                name: "EventName",
                table: "Events",
                newName: "Name");
        }
    }
}
