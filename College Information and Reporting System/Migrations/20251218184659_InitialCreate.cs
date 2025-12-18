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
                name: "departments",
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
                    table.PrimaryKey("PK_departments", x => x.departmentId);
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
                name: "courses",
                columns: table => new
                {
                    courseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    startYear = table.Column<DateOnly>(type: "date", nullable: false),
                    departmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.courseId);
                    table.ForeignKey(
                        name: "FK_courses_departments_departmentId",
                        column: x => x.departmentId,
                        principalTable: "departments",
                        principalColumn: "departmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attendances",
                columns: table => new
                {
                    attendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    attendanceTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    attendanceStatus = table.Column<int>(type: "int", nullable: false),
                    studentId = table.Column<int>(type: "int", nullable: false),
                    courseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendances", x => x.attendanceId);
                    table.ForeignKey(
                        name: "FK_attendances_courses_courseId",
                        column: x => x.courseId,
                        principalTable: "courses",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attendances_students_studentId",
                        column: x => x.studentId,
                        principalTable: "students",
                        principalColumn: "studentId",
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
                        name: "FK_CourseStudent_courses_coursescourseId",
                        column: x => x.coursescourseId,
                        principalTable: "courses",
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
                name: "IX_attendances_courseId",
                table: "attendances",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_attendances_studentId",
                table: "attendances",
                column: "studentId");

            migrationBuilder.CreateIndex(
                name: "IX_courses_departmentId",
                table: "courses",
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
                name: "attendances");

            migrationBuilder.DropTable(
                name: "CourseStudent");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
