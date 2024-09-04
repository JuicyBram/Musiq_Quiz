using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MusicQuiz.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseReset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FSAudioFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullSongBase64 = table.Column<byte[]>(type: "bytea", nullable: false),
                    FullSongDuration = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSAudioFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GSAudioFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuitarSoloBase64 = table.Column<byte[]>(type: "bytea", nullable: false),
                    GituarSoloDuration = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GSAudioFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SongNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FSAudioFileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SongNames_FSAudioFiles_FSAudioFileId",
                        column: x => x.FSAudioFileId,
                        principalTable: "FSAudioFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongNames_FSAudioFileId",
                table: "SongNames",
                column: "FSAudioFileId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GSAudioFiles");

            migrationBuilder.DropTable(
                name: "SongNames");

            migrationBuilder.DropTable(
                name: "FSAudioFiles");
        }
    }
}
