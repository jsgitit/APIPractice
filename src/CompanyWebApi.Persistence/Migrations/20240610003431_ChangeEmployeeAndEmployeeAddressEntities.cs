using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyWebApi.Persistence.Migrations
{
    public partial class ChangeEmployeeAndEmployeeAddressEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeAddresses",
                table: "EmployeeAddresses");

            migrationBuilder.DeleteData(
                table: "EmployeeAddresses",
                keyColumn: "EmployeeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EmployeeAddresses",
                keyColumn: "EmployeeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EmployeeAddresses",
                keyColumn: "EmployeeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EmployeeAddresses",
                keyColumn: "EmployeeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EmployeeAddresses",
                keyColumn: "EmployeeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EmployeeAddresses",
                keyColumn: "EmployeeId",
                keyValue: 6);

            migrationBuilder.AddColumn<int>(
                name: "AddressTypeId",
                table: "EmployeeAddresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "EmployeeAddresses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "EmployeeAddresses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeAddresses",
                table: "EmployeeAddresses",
                columns: new[] { "EmployeeId", "AddressTypeId" });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "AddressTypeId", "EmployeeId", "Address", "Created", "Modified" },
                values: new object[] { 1, 1, "Kentucky, USA", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "AddressTypeId", "EmployeeId", "Address", "Created", "Modified" },
                values: new object[] { 1, 2, "Berlin, Germany", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "AddressTypeId", "EmployeeId", "Address", "Created", "Modified" },
                values: new object[] { 1, 3, "Los Angeles, USA", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "AddressTypeId", "EmployeeId", "Address", "Created", "Modified" },
                values: new object[] { 1, 4, "Vienna, Austria", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "AddressTypeId", "EmployeeId", "Address", "Created", "Modified" },
                values: new object[] { 1, 5, "Cologne, Germany", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "AddressTypeId", "EmployeeId", "Address", "Created", "Modified" },
                values: new object[] { 1, 6, "Milano, Italy", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeAddresses",
                table: "EmployeeAddresses");

            migrationBuilder.DropColumn(
                name: "AddressTypeId",
                table: "EmployeeAddresses");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "EmployeeAddresses");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "EmployeeAddresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeAddresses",
                table: "EmployeeAddresses",
                column: "EmployeeId");
        }
    }
}
