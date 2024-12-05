using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveySystem.API.Migrations
{
    /// <inheritdoc />
    public partial class FixAnswerConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_questions_IdQuestion",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "FK_answers_questions_QuestionId",
                table: "answers");

            migrationBuilder.DropIndex(
                name: "IX_answers_IdQuestion",
                table: "answers");

            migrationBuilder.DropColumn(
                name: "IdQuestion",
                table: "answers");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "answers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_questions_QuestionId",
                table: "answers",
                column: "QuestionId",
                principalTable: "questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_questions_QuestionId",
                table: "answers");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "answers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "IdQuestion",
                table: "answers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_answers_IdQuestion",
                table: "answers",
                column: "IdQuestion");

            migrationBuilder.AddForeignKey(
                name: "FK_answers_questions_IdQuestion",
                table: "answers",
                column: "IdQuestion",
                principalTable: "questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_questions_QuestionId",
                table: "answers",
                column: "QuestionId",
                principalTable: "questions",
                principalColumn: "Id");
        }
    }
}
