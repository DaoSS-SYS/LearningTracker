using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningTracker.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class ResctrictDeletingTopics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudySessions_LearningTopics_LearningTopicId",
                table: "StudySessions");

            migrationBuilder.AddForeignKey(
                name: "FK_StudySessions_LearningTopics_LearningTopicId",
                table: "StudySessions",
                column: "LearningTopicId",
                principalTable: "LearningTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudySessions_LearningTopics_LearningTopicId",
                table: "StudySessions");

            migrationBuilder.AddForeignKey(
                name: "FK_StudySessions_LearningTopics_LearningTopicId",
                table: "StudySessions",
                column: "LearningTopicId",
                principalTable: "LearningTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
