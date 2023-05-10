using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnywhereFit.Migrations
{
    /// <inheritdoc />
    public partial class BreakoutExerciseModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GifUrl",
                table: "Exercises");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GifUrl",
                table: "Exercises",
                type: "TEXT",
                nullable: true);
        }
    }
}
