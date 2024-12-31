using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class reviewcount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "BookReviews",
                newName: "BookReviews",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "BookRanks",
                newName: "BookRanks",
                newSchema: "public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "BookReviews",
                schema: "public",
                newName: "BookReviews");

            migrationBuilder.RenameTable(
                name: "BookRanks",
                schema: "public",
                newName: "BookRanks");
        }
    }
}
