using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vote_API.Migrations
{
    public partial class SpotifyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpotifyPlaylistId",
                table: "VoteablePlaylistDto",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotifyPlaylistId",
                table: "VoteablePlaylistDto");
        }
    }
}
