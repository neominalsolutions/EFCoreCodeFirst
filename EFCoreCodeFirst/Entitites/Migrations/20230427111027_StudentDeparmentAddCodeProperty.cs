using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreCodeFirst.Entitites.Migrations
{
    public partial class StudentDeparmentAddCodeProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "StudentDepartments",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "StudentDepartments");
        }
    }
}
