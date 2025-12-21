using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ClaimLabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_method_claims",
                table: "method_claims");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "method_claims",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_method_claims",
                table: "method_claims",
                columns: new[] { "UserId", "Label", "MethodId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_method_claims",
                table: "method_claims");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "method_claims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_method_claims",
                table: "method_claims",
                columns: new[] { "UserId", "MethodId" });
        }
    }
}
