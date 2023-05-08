using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnywhereFit.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExerciseClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Exercises",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Exercises");
        }
    }
}
