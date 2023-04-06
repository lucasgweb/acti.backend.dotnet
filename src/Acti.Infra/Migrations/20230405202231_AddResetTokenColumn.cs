using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManager.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddResetTokenColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "resetToken",
                table: "users",
                type: "VARCHAR(180)",
                maxLength: 180,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resetToken",
                table: "users");
        }
    }
}
