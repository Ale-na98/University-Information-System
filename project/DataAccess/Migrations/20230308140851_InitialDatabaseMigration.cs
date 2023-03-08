using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class InitialDatabaseMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_name = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.group_id);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    teacher_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "varchar", nullable: false),
                    email = table.Column<string>(type: "varchar", nullable: false),
                    phone_number = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.teacher_id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "varchar", nullable: false),
                    email = table.Column<string>(type: "varchar", nullable: false),
                    phone_number = table.Column<string>(type: "varchar", nullable: false),
                    group_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.student_id);
                    table.ForeignKey(
                        name: "FK_students_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "group_id");
                });

            migrationBuilder.CreateTable(
                name: "lectures",
                columns: table => new
                {
                    lecture_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lecture_name = table.Column<string>(type: "varchar", nullable: false),
                    teacher_id = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lectures", x => x.lecture_id);
                    table.ForeignKey(
                        name: "FK_lectures_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "teacher_id");
                });

            migrationBuilder.CreateTable(
                name: "attendance",
                columns: table => new
                {
                    attendance_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lecture_id = table.Column<int>(type: "integer", nullable: false),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    presence = table.Column<bool>(type: "boolean", nullable: false),
                    hometask_done = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance", x => x.attendance_id);
                    table.ForeignKey(
                        name: "FK_attendance_lectures_lecture_id",
                        column: x => x.lecture_id,
                        principalTable: "lectures",
                        principalColumn: "lecture_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attendance_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hometasks",
                columns: table => new
                {
                    hometask_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    mark = table.Column<short>(type: "smallint", nullable: false),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    lecture_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hometasks", x => x.hometask_id);
                    table.ForeignKey(
                        name: "FK_hometasks_lectures_lecture_id",
                        column: x => x.lecture_id,
                        principalTable: "lectures",
                        principalColumn: "lecture_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_hometasks_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedule",
                columns: table => new
                {
                    lecture_id = table.Column<int>(type: "integer", nullable: false),
                    group_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule", x => new { x.lecture_id, x.group_id });
                    table.ForeignKey(
                        name: "FK_schedule_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_schedule_lectures_lecture_id",
                        column: x => x.lecture_id,
                        principalTable: "lectures",
                        principalColumn: "lecture_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "groups",
                columns: new[] { "group_id", "group_name" },
                values: new object[,]
                {
                    { 1, "6501" },
                    { 2, "6502" },
                    { 3, "6503" }
                });

            migrationBuilder.InsertData(
                table: "teachers",
                columns: new[] { "teacher_id", "email", "full_name", "phone_number" },
                values: new object[,]
                {
                    { 1, "ivanivanov@gmail.com", "Ivan Ivanov", "89166975213" },
                    { 2, "peterpetrov@gmail.com", "Peter Petrov", "88166975649" },
                    { 3, "peterivanov@gmail.com", "Peter Ivanov", "88126932678" },
                    { 4, "larisamironova@gmail.com", "Larisa Mironova", "89177875231" },
                    { 5, "nikolaygirin@gmail.com", "Nikolay Girin", "88496575634" },
                    { 6, "galinasemenova@gmail.com", "Galina Semenova", "88166991679" }
                });

            migrationBuilder.InsertData(
                table: "lectures",
                columns: new[] { "lecture_id", "lecture_name", "teacher_id" },
                values: new object[,]
                {
                    { 1, "Maths", (short)1 },
                    { 2, "Probability theory", (short)1 },
                    { 3, "Physics", (short)2 },
                    { 4, "History", (short)3 },
                    { 5, "Biology", (short)4 },
                    { 6, "Chemistry", (short)5 },
                    { 7, "English", (short)6 }
                });

            migrationBuilder.InsertData(
                table: "students",
                columns: new[] { "student_id", "email", "full_name", "group_id", "phone_number" },
                values: new object[,]
                {
                    { 1, "romankozlov@gmail.com", "Roman Kozlov", 1, "89169475896" },
                    { 2, "maximzakharov@gmail.com", "Maxim Zakharov", 1, "89129475994" },
                    { 3, "kirillmorozov@gmail.com", "Kirill Morozov", 1, "89159475789" },
                    { 4, "ivansvistunov@gmail.com", "Ivan Svistunov", 1, "89129491486" },
                    { 5, "igorsokolov@gmail.com", "Igor Sokolov", 2, "88329445692" },
                    { 6, "sergeykoshkin@gmail.com", "Sergey Koshkin", 2, "89169435789" },
                    { 7, "anastasiasoboleva@gmail.com", "Anastasia Soboleva", 2, "89139435787" },
                    { 8, "kristinakereeva@gmail.com", "Kristina Kereeva", 2, "83129438452" },
                    { 9, "marialarina@gmail.com", "Maria Larina", 3, "89459445623" },
                    { 10, "olgasmirnova@gmail.com", "Olga Smirnova", 3, "89147835231" },
                    { 11, "viktormironov@gmail.com", "Viktor Mironov", 3, "89157535221" },
                    { 12, "konstantingusev@gmail.com", "Konstantin Gusev", 3, "89127534610" }
                });

            migrationBuilder.InsertData(
                table: "attendance",
                columns: new[] { "attendance_id", "hometask_done", "date", "lecture_id", "presence", "student_id" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 1 },
                    { 2, false, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 2 },
                    { 3, true, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, 3 },
                    { 4, false, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 4 },
                    { 5, false, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 5 },
                    { 6, true, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 6 },
                    { 7, false, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 7 },
                    { 8, true, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 8 },
                    { 9, true, new DateTime(2013, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 9 },
                    { 10, false, new DateTime(2013, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, 10 },
                    { 11, true, new DateTime(2013, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 11 },
                    { 12, false, new DateTime(2013, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 12 },
                    { 13, false, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, true, 1 },
                    { 14, false, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, false, 2 },
                    { 15, true, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, true, 3 },
                    { 16, false, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, false, 4 },
                    { 17, true, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, true, 1 },
                    { 18, false, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, true, 2 },
                    { 19, true, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, true, 3 },
                    { 20, false, new DateTime(2013, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, false, 4 },
                    { 21, true, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, true, 5 },
                    { 22, false, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, false, 6 },
                    { 23, true, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, true, 7 },
                    { 24, false, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, true, 8 },
                    { 25, true, new DateTime(2013, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, true, 9 },
                    { 26, false, new DateTime(2013, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, true, 10 },
                    { 27, true, new DateTime(2013, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, true, 11 },
                    { 28, false, new DateTime(2013, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, true, 12 },
                    { 29, true, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, true, 5 },
                    { 30, true, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, true, 6 },
                    { 31, true, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, true, 7 },
                    { 32, true, new DateTime(2013, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, true, 8 }
                });

            migrationBuilder.InsertData(
                table: "hometasks",
                columns: new[] { "hometask_id", "date", "lecture_id", "mark", "student_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2013, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, (short)4, 1 },
                    { 2, new DateTime(2013, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, (short)5, 2 },
                    { 3, new DateTime(2013, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, (short)3, 1 },
                    { 4, new DateTime(2013, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, (short)4, 2 }
                });

            migrationBuilder.InsertData(
                table: "schedule",
                columns: new[] { "group_id", "lecture_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 4 },
                    { 3, 5 },
                    { 3, 6 },
                    { 2, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_lecture_id",
                table: "attendance",
                column: "lecture_id");

            migrationBuilder.CreateIndex(
                name: "IX_attendance_student_id_lecture_id_date",
                table: "attendance",
                columns: new[] { "student_id", "lecture_id", "date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_groups_group_name",
                table: "groups",
                column: "group_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hometasks_lecture_id",
                table: "hometasks",
                column: "lecture_id");

            migrationBuilder.CreateIndex(
                name: "IX_hometasks_student_id",
                table: "hometasks",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_lectures_teacher_id",
                table: "lectures",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_group_id",
                table: "schedule",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_email",
                table: "students",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_group_id",
                table: "students",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_phone_number",
                table: "students",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teachers_email",
                table: "teachers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teachers_phone_number",
                table: "teachers",
                column: "phone_number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attendance");

            migrationBuilder.DropTable(
                name: "hometasks");

            migrationBuilder.DropTable(
                name: "schedule");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "lectures");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropTable(
                name: "teachers");
        }
    }
}
