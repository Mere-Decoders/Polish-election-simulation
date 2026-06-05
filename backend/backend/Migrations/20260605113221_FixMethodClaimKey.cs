using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class FixMethodClaimKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_method_claims",
                table: "method_claims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_method_claims",
                table: "method_claims",
                columns: new[] { "UserId", "MethodId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_method_claims",
                table: "method_claims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_method_claims",
                table: "method_claims",
                columns: new[] { "UserId", "Label", "MethodId" });
        }
    }
}
