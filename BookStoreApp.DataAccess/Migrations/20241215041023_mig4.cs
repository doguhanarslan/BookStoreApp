using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookAuthor",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "BookDescription",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "BookImage",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Carts",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CartItems",
                newSchema: "public");

            migrationBuilder.RenameColumn(
                name: "BookTitle",
                schema: "public",
                table: "CartItems",
                newName: "CartSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_BookId",
                schema: "public",
                table: "CartItems",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Book_BookId",
                schema: "public",
                table: "CartItems",
                column: "BookId",
                principalSchema: "public",
                principalTable: "Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Book_BookId",
                schema: "public",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_BookId",
                schema: "public",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "Carts",
                schema: "public",
                newName: "Carts");

            migrationBuilder.RenameTable(
                name: "CartItems",
                schema: "public",
                newName: "CartItems");

            migrationBuilder.RenameColumn(
                name: "CartSessionId",
                table: "CartItems",
                newName: "BookTitle");

            migrationBuilder.AddColumn<string>(
                name: "BookAuthor",
                table: "CartItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BookDescription",
                table: "CartItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BookImage",
                table: "CartItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "CartItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
