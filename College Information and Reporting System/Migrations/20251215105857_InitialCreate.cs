using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace College_Information_and_Reporting_System.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    departmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    departmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departmentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departmentHead = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.departmentId);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    studentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentGender = table.Column<int>(type: "int", nullable: false),
                    studentDateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    studentEthnicity = table.Column<int>(type: "int", nullable: false),
                    studentStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    studentEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.studentId);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    courseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    startYear = table.Column<DateOnly>(type: "date", nullable: false),
                    departmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.courseId);
                    table.ForeignKey(
                        name: "FK_Course_Department_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Department",
                        principalColumn: "departmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseStudent",
                columns: table => new
                {
                    coursescourseId = table.Column<int>(type: "int", nullable: false),
                    studentsstudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudent", x => new { x.coursescourseId, x.studentsstudentId });
                    table.ForeignKey(
                        name: "FK_CourseStudent_Course_coursescourseId",
                        column: x => x.coursescourseId,
                        principalTable: "Course",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStudent_students_studentsstudentId",
                        column: x => x.studentsstudentId,
                        principalTable: "students",
                        principalColumn: "studentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_departmentId",
                table: "Course",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudent_studentsstudentId",
                table: "CourseStudent",
                column: "studentsstudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseStudent");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
