using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramLearningBot.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsersId = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_themes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_themes_users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dictionaryies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ThemesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionaryies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dictionaryies_themes_ThemesId",
                        column: x => x.ThemesId,
                        principalTable: "themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ThemesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tests_themes_ThemesId",
                        column: x => x.ThemesId,
                        principalTable: "themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearningWord = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TranslateOfWord = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DictionaryiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_words_dictionaryies_DictionaryiesId",
                        column: x => x.DictionaryiesId,
                        principalTable: "dictionaryies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    questionText = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    AnswerText = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    TestsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_questions_tests_TestsId",
                        column: x => x.TestsId,
                        principalTable: "tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dictionaryies_ThemesId",
                table: "dictionaryies",
                column: "ThemesId");

            migrationBuilder.CreateIndex(
                name: "IX_questions_TestsId",
                table: "questions",
                column: "TestsId");

            migrationBuilder.CreateIndex(
                name: "IX_tests_ThemesId",
                table: "tests",
                column: "ThemesId");

            migrationBuilder.CreateIndex(
                name: "IX_themes_UsersId",
                table: "themes",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_words_DictionaryiesId",
                table: "words",
                column: "DictionaryiesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "words");

            migrationBuilder.DropTable(
                name: "tests");

            migrationBuilder.DropTable(
                name: "dictionaryies");

            migrationBuilder.DropTable(
                name: "themes");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
