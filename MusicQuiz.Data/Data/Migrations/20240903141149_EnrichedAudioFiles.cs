using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MusicQuiz.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnrichedAudioFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AudioFiles");

            migrationBuilder.RenameColumn(
                name: "AudioData",
                table: "AudioFiles",
                newName: "GituarSoloBase64");

            migrationBuilder.AddColumn<byte[]>(
                name: "FullSongBase64",
                table: "AudioFiles",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<float>(
                name: "FullSongDuration",
                table: "AudioFiles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "GituarSoloDuration",
                table: "AudioFiles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "SongNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AudioFileId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SongNames_AudioFiles_AudioFileId",
                        column: x => x.AudioFileId,
                        principalTable: "AudioFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongNames_AudioFileId",
                table: "SongNames",
                column: "AudioFileId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongNames");

            migrationBuilder.DropColumn(
                name: "FullSongBase64",
                table: "AudioFiles");

            migrationBuilder.DropColumn(
                name: "FullSongDuration",
                table: "AudioFiles");

            migrationBuilder.DropColumn(
                name: "GituarSoloDuration",
                table: "AudioFiles");

            migrationBuilder.RenameColumn(
                name: "GituarSoloBase64",
                table: "AudioFiles",
                newName: "AudioData");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AudioFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
