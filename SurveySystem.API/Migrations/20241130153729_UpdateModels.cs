using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveySystem.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId1",
                table: "answers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_answers_QuestionId1",
                table: "answers",
                column: "QuestionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_answers_questions_QuestionId1",
                table: "answers",
                column: "QuestionId1",
                principalTable: "questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_questions_QuestionId1",
                table: "answers");

            migrationBuilder.DropIndex(
                name: "IX_answers_QuestionId1",
                table: "answers");

            migrationBuilder.DropColumn(
                name: "QuestionId1",
                table: "answers");
        }
    }
}
