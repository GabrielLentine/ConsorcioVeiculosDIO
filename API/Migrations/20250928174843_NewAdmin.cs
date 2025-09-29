using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsorcioVeiculos.Migrations
{
    /// <inheritdoc />
    public partial class NewAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Perfil",
                table: "Administradores",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Administradores",
                keyColumn: "Id",
                keyValue: 1,
                column: "Perfil",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Perfil",
                table: "Administradores",
                type: "int",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
