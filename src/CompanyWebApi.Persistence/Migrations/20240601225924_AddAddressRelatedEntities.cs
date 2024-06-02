using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyWebApi.Persistence.Migrations
{
    public partial class AddAddressRelatedEntities : Migration
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

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1524), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1525) });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 2,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1526), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1526) });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 3,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1527), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1527) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1603), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1603) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1604), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1604) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1605), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1605) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1606), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1606) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 5,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1607), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1607) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 6,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1607), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1608) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 7,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1608), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1608) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 8,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1609), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1609) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 9,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1610), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1610) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 10,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1610), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1610) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 11,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1611), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1611) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1623), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1623) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 2,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1626), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1626) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 3,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1627), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1628) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 4,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1628), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1628) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 5,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1629), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1629) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 6,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1630), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1630) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 1,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1650), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1650) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 2,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1651), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1651) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 3,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1652), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1652) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 4,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1652), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1652) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 5,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1653), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1653) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 6,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1653), new DateTime(2024, 6, 1, 22, 59, 23, 808, DateTimeKind.Utc).AddTicks(1654) });

            migrationBuilder.CreateIndex(
                name: "IX_AddressRelations_AddressId",
                table: "AddressRelations",
                column: "AddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressRelations");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9909), new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9910) });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 2,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9913), new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9913) });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 3,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9913), new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9914) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(1), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(2) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(2), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(2) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(4), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(4) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 5,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(5), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(5) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 6,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(5), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(6) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 7,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(6), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(7) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 8,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(7), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(7) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 9,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(8), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(8) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 10,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(9), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(9) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 11,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(9), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(10) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(24), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(24) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 2,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(27), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(27) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 3,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(28), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(28) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 4,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(29), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(29) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 5,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(30), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(30) });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 6,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(31), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(31) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 1,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(60), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(60) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 2,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(61), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(61) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 3,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(62), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(62) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 4,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(62), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(63) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 5,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(63), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(63) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "EmployeeId",
                keyValue: 6,
                columns: new[] { "Created", "Modified" },
                values: new object[] { new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(66), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(66) });
        }
    }
}
