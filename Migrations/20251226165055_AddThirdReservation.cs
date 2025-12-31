using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemeHotel.Migrations
{
    /// <inheritdoc />
    public partial class AddThirdReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "ChambreId", "ClientId", "DateDebut", "DateFin", "Statut" },
                values: new object[] { 3, 3, 3, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
