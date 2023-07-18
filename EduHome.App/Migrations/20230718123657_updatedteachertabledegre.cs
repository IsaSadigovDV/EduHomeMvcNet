using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.App.Migrations
{
    public partial class updatedteachertabledegre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sliders_Teachers_TeacherId",
                table: "Sliders");

            migrationBuilder.DropIndex(
                name: "IX_Sliders_TeacherId",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Sliders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Sliders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sliders_TeacherId",
                table: "Sliders",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sliders_Teachers_TeacherId",
                table: "Sliders",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
