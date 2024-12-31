using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class rating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRanks_BookReviews_BookReviewId",
                table: "BookRanks");

            migrationBuilder.DropIndex(
                name: "IX_BookRanks_BookReviewId",
                table: "BookRanks");

            migrationBuilder.DropColumn(
                name: "BookReviewId",
                table: "BookRanks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookReviewId",
                table: "BookRanks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookRanks_BookReviewId",
                table: "BookRanks",
                column: "BookReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRanks_BookReviews_BookReviewId",
                table: "BookRanks",
                column: "BookReviewId",
                principalTable: "BookReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
