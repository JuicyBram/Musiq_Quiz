using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicQuiz.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedSpellingMistake : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GituarSoloBase64",
                table: "AudioFiles",
                newName: "GuitarSoloBase64");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GuitarSoloBase64",
                table: "AudioFiles",
                newName: "GituarSoloBase64");
        }
    }
}
