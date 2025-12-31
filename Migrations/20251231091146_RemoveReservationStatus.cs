using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SystemeHotel.Migrations
{
    /// <inheritdoc />
    public partial class RemoveReservationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Reservations");

            migrationBuilder.InsertData(
                table: "Factures",
                columns: new[] { "Id", "DateEmission", "ReservationId", "Statut", "Total" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "En attente", 100.0 },
                    { 2, new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Payée", 240.0 },
                    { 3, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "En attente", 300.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Factures",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Factures",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Factures",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "Statut",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Statut",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Statut",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Statut",
                value: 1);
        }
    }
}
