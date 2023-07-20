using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.App.Migrations
{
    public partial class updatedcoursetableassesment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_courseAssests_CourseAssestsId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "courseAssests");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseAssestsId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseAssestsId",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Assesments",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assesments",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CourseAssestsId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "courseAssests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courseAssests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseAssestsId",
                table: "Courses",
                column: "CourseAssestsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_courseAssests_CourseAssestsId",
                table: "Courses",
                column: "CourseAssestsId",
                principalTable: "courseAssests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
