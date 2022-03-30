using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vote_API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VoteData",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AuthorUserUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ValidUntil = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    JoinCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Salt = table.Column<byte[]>(type: "longblob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteData", x => x.Uuid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VoteablePlaylistDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    VoteDataUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SpotifyPlaylistId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlaylistName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlaylistImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteablePlaylistDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_VoteablePlaylistDto_VoteData_VoteDataUuid",
                        column: x => x.VoteDataUuid,
                        principalTable: "VoteData",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlaylistVote",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    VoteDataUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SpotifyPlaylistUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistVote", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PlaylistVote_VoteablePlaylistDto_VoteDataUuid",
                        column: x => x.VoteDataUuid,
                        principalTable: "VoteablePlaylistDto",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SpotifyPlaylistSongDto",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PlaylistUuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SongName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArtistName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SongImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyPlaylistSongDto", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_SpotifyPlaylistSongDto_VoteablePlaylistDto_PlaylistUuid",
                        column: x => x.PlaylistUuid,
                        principalTable: "VoteablePlaylistDto",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistVote_VoteDataUuid",
                table: "PlaylistVote",
                column: "VoteDataUuid");

            migrationBuilder.CreateIndex(
                name: "IX_SpotifyPlaylistSongDto_PlaylistUuid",
                table: "SpotifyPlaylistSongDto",
                column: "PlaylistUuid");

            migrationBuilder.CreateIndex(
                name: "IX_VoteablePlaylistDto_VoteDataUuid",
                table: "VoteablePlaylistDto",
                column: "VoteDataUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistVote");

            migrationBuilder.DropTable(
                name: "SpotifyPlaylistSongDto");

            migrationBuilder.DropTable(
                name: "VoteablePlaylistDto");

            migrationBuilder.DropTable(
                name: "VoteData");
        }
    }
}
