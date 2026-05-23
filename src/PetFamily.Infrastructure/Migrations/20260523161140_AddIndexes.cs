using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TUTORES_EMAIL",
                table: "TUTORES",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PETS_ESPECIE",
                table: "PETS",
                column: "ESPECIE");

            migrationBuilder.CreateIndex(
                name: "IX_LEMBRETES_DATA",
                table: "LEMBRETES",
                column: "DATA_LEMBRETE");

            migrationBuilder.CreateIndex(
                name: "IX_CONSULTAS_DATA",
                table: "CONSULTAS",
                column: "DATA_CONSULTA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TUTORES_EMAIL",
                table: "TUTORES");

            migrationBuilder.DropIndex(
                name: "IX_PETS_ESPECIE",
                table: "PETS");

            migrationBuilder.DropIndex(
                name: "IX_LEMBRETES_DATA",
                table: "LEMBRETES");

            migrationBuilder.DropIndex(
                name: "IX_CONSULTAS_DATA",
                table: "CONSULTAS");
        }
    }
}
