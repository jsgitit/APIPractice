using System;
using CompanyWebApi.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyWebApi.Persistence.Migrations
{
    public partial class AddAddressEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FullAddress = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "AddressRelations",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EntityIdType = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressRelations", x => new { x.EntityIdType, x.EntityId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_AddressRelations_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressRelations_AddressId",
                table: "AddressRelations",
                column: "AddressId");

            var baseGuid = "db32bfaf-c0cd-4755-a1b5-00000000000"; // one-digit short of full guid.

            // Copy data from EmployeeAddress to Addresses and AddressRelations using manual inserts for now
            migrationBuilder.Sql(@"
                INSERT INTO Addresses (AddressId, FullAddress, Created, Modified)
                SELECT '" + baseGuid + @"1', Address, DATETIME('now'), DATETIME('1970-01-01') FROM EmployeeAddresses
                WHERE EmployeeId = 1;
                INSERT INTO Addresses (AddressId, FullAddress, Created, Modified)
                SELECT '" + baseGuid + @"2', Address, DATETIME('now'), DATETIME('1970-01-01') FROM EmployeeAddresses
                WHERE EmployeeId = 2;
                INSERT INTO Addresses (AddressId, FullAddress, Created, Modified)
                SELECT '" + baseGuid + @"3', Address, DATETIME('now'), DATETIME('1970-01-01') FROM EmployeeAddresses
                WHERE EmployeeId = 3;
                INSERT INTO Addresses (AddressId, FullAddress, Created, Modified)
                SELECT '" + baseGuid + @"4', Address, DATETIME('now'), DATETIME('1970-01-01') FROM EmployeeAddresses
                WHERE EmployeeId = 4;
                INSERT INTO Addresses (AddressId, FullAddress, Created, Modified)
                SELECT '" + baseGuid + @"5', Address, DATETIME('now'), DATETIME('1970-01-01') FROM EmployeeAddresses
                WHERE EmployeeId = 5;
                INSERT INTO Addresses (AddressId, FullAddress, Created, Modified)
                SELECT '" + baseGuid + @"6', Address, DATETIME('now'), DATETIME('1970-01-01') FROM EmployeeAddresses
                WHERE EmployeeId = 6;

                INSERT INTO AddressRelations (AddressId, EntityIdType, EntityId)
                SELECT AddressId, 3, 1 FROM Addresses
                WHERE AddressId = '" + baseGuid + @"1';
                INSERT INTO AddressRelations (AddressId, EntityIdType, EntityId)
                SELECT AddressId, 3, 2 FROM Addresses
                WHERE AddressId = '" + baseGuid + @"2';
                INSERT INTO AddressRelations (AddressId, EntityIdType, EntityId)
                SELECT AddressId, 3, 3 FROM Addresses
                WHERE AddressId = '" + baseGuid + @"3';
                INSERT INTO AddressRelations (AddressId, EntityIdType, EntityId)
                SELECT AddressId, 3, 4 FROM Addresses
                WHERE AddressId = '" + baseGuid + @"4';
                INSERT INTO AddressRelations (AddressId, EntityIdType, EntityId)
                SELECT AddressId, 3, 5 FROM Addresses
                WHERE AddressId = '" + baseGuid + @"5';
                INSERT INTO AddressRelations (AddressId, EntityIdType, EntityId)
                SELECT AddressId, 3, 6 FROM Addresses
                WHERE AddressId = '" + baseGuid + @"6';
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressRelations");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
