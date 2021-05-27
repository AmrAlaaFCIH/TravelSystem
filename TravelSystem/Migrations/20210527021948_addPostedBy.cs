using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelSystem.Migrations
{
    public partial class addPostedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "TripPosts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripPosts_OwnerID",
                table: "TripPosts",
                column: "OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_TripPosts_AspNetUsers_OwnerID",
                table: "TripPosts",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripPosts_AspNetUsers_OwnerID",
                table: "TripPosts");

            migrationBuilder.DropIndex(
                name: "IX_TripPosts_OwnerID",
                table: "TripPosts");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "TripPosts");
        }
    }
}
