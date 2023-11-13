using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Semester",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semester", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Dni = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    ImageUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseBySemester",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    SemesterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseBySemester", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CourseByS__Cours__3587F3E0",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseBySemester_Semester",
                        column: x => x.SemesterId,
                        principalTable: "Semester",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserInformationId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    AvatarUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_UserInformation",
                        column: x => x.UserInformationId,
                        principalTable: "UserInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseBySemesterEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    WeightPercentage = table.Column<int>(type: "int", nullable: false),
                    CourseBySemesterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseBySemesterEvaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseBySemesterEvaluation_CourseBySemester",
                        column: x => x.CourseBySemesterId,
                        principalTable: "CourseBySemester",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseBySemesterId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedule_CourseBySemester",
                        column: x => x.CourseBySemesterId,
                        principalTable: "CourseBySemester",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserByType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserByType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserByType_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserByType_UserType",
                        column: x => x.UserTypeId,
                        principalTable: "UserType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseBySemesterEnroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseBySemesterId = table.Column<int>(type: "int", nullable: false),
                    UserByTypeStudentId = table.Column<int>(type: "int", nullable: false),
                    UserByTypeTeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseBySemesterEnroll", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseBySemesterEnroll_CourseBySemester",
                        column: x => x.CourseBySemesterId,
                        principalTable: "CourseBySemester",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseBySemesterEnroll_UserByTypeStudent",
                        column: x => x.UserByTypeStudentId,
                        principalTable: "UserByType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseBySemesterEnroll_UserByTypeTeacher",
                        column: x => x.UserByTypeTeacherId,
                        principalTable: "UserByType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PublishedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    UserByTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_UserByType",
                        column: x => x.UserByTypeId,
                        principalTable: "UserByType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    CourseBySemesterEvaluationId = table.Column<int>(type: "int", nullable: false),
                    CourseBySemesterEnrollId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Note_CourseBySemesterEnroll",
                        column: x => x.CourseBySemesterEnrollId,
                        principalTable: "CourseBySemesterEnroll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Note_CourseBySemesterEvaluation",
                        column: x => x.CourseBySemesterEvaluationId,
                        principalTable: "CourseBySemesterEvaluation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemester_CourseId",
                table: "CourseBySemester",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemester_SemesterId",
                table: "CourseBySemester",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemesterEnroll_CourseBySemesterId",
                table: "CourseBySemesterEnroll",
                column: "CourseBySemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemesterEnroll_UserByTypeStudentId",
                table: "CourseBySemesterEnroll",
                column: "UserByTypeStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemesterEnroll_UserByTypeTeacherId",
                table: "CourseBySemesterEnroll",
                column: "UserByTypeTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemesterEvaluation_CourseBySemesterId",
                table: "CourseBySemesterEvaluation",
                column: "CourseBySemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_CourseBySemesterEnrollId",
                table: "Note",
                column: "CourseBySemesterEnrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_CourseBySemesterEvaluationId",
                table: "Note",
                column: "CourseBySemesterEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserByTypeId",
                table: "Post",
                column: "UserByTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_CourseBySemesterId",
                table: "Schedule",
                column: "CourseBySemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserInformationId",
                table: "User",
                column: "UserInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserByType_UserId",
                table: "UserByType",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserByType_UserTypeId",
                table: "UserByType",
                column: "UserTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "CourseBySemesterEnroll");

            migrationBuilder.DropTable(
                name: "CourseBySemesterEvaluation");

            migrationBuilder.DropTable(
                name: "UserByType");

            migrationBuilder.DropTable(
                name: "CourseBySemester");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserType");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Semester");

            migrationBuilder.DropTable(
                name: "UserInformation");
        }
    }
}
