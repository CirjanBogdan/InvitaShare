using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvitaShare.Data.Migrations
{
    /// <inheritdoc />
    public partial class EventCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeddingEvent_RestaurantName",
                table: "Events",
                newName: "PartnerName2");

            migrationBuilder.AddColumn<string>(
                name: "ParentName1",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentName2",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerName1",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentName1",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ParentName2",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PartnerName1",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "PartnerName2",
                table: "Events",
                newName: "WeddingEvent_RestaurantName");
        }
    }
}
