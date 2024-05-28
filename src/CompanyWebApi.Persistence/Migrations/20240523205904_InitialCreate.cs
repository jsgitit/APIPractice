﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyWebApi.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAddresses",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAddresses", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_EmployeeAddresses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Users_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 1, new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9909), new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9910), "Company One" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 2, new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9913), new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9913), "Company Two" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 3, new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9913), new DateTime(2024, 5, 23, 20, 59, 3, 935, DateTimeKind.Utc).AddTicks(9914), "Company Three" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 1, 1, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc), "Logistics" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 2, 1, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(1), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(2), "Administration" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 3, 1, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(2), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(2), "Development" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 4, 2, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(4), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(4), "Sales" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 5, 2, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(5), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(5), "Marketing" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 6, 3, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(5), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(6), "Customer support" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 7, 3, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(6), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(7), "Research and Development" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 8, 3, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(7), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(7), "Purchasing" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 9, 3, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(8), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(8), "Human Resource Management" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 10, 3, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(9), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(9), "Accounting and Finance" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CompanyId", "Created", "Modified", "Name" },
                values: new object[] { 11, 3, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(9), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(10), "Production" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "BirthDate", "CompanyId", "Created", "DepartmentId", "FirstName", "LastName", "Modified" },
                values: new object[] { 1, new DateTime(1991, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(24), 1, "John", "Whyne", new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(24) });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "BirthDate", "CompanyId", "Created", "DepartmentId", "FirstName", "LastName", "Modified" },
                values: new object[] { 2, new DateTime(1997, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(27), 4, "Mathias", "Gernold", new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(27) });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "BirthDate", "CompanyId", "Created", "DepartmentId", "FirstName", "LastName", "Modified" },
                values: new object[] { 3, new DateTime(1955, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(28), 7, "Julia", "Reynolds", new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(28) });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "BirthDate", "CompanyId", "Created", "DepartmentId", "FirstName", "LastName", "Modified" },
                values: new object[] { 4, new DateTime(1935, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(29), 2, "Alois", "Mock", new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(29) });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "BirthDate", "CompanyId", "Created", "DepartmentId", "FirstName", "LastName", "Modified" },
                values: new object[] { 5, new DateTime(2001, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(30), 6, "Gertraud", "Bochold", new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(30) });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "BirthDate", "CompanyId", "Created", "DepartmentId", "FirstName", "LastName", "Modified" },
                values: new object[] { 6, new DateTime(1984, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(31), 6, "Alan", "Ford", new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(31) });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "EmployeeId", "Address" },
                values: new object[] { 1, "Kentucky, USA" });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "EmployeeId", "Address" },
                values: new object[] { 2, "Berlin, Germany" });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "EmployeeId", "Address" },
                values: new object[] { 3, "Los Angeles, USA" });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "EmployeeId", "Address" },
                values: new object[] { 4, "Vienna, Austria" });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "EmployeeId", "Address" },
                values: new object[] { 5, "Cologne, Germany" });

            migrationBuilder.InsertData(
                table: "EmployeeAddresses",
                columns: new[] { "EmployeeId", "Address" },
                values: new object[] { 6, "Milano, Italy" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "EmployeeId", "Created", "Modified", "Password", "Token", "Username" },
                values: new object[] { 1, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(60), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(60), "test", "", "johnw" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "EmployeeId", "Created", "Modified", "Password", "Token", "Username" },
                values: new object[] { 2, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(61), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(61), "test", "", "mathiasg" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "EmployeeId", "Created", "Modified", "Password", "Token", "Username" },
                values: new object[] { 3, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(62), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(62), "test", "", "juliar" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "EmployeeId", "Created", "Modified", "Password", "Token", "Username" },
                values: new object[] { 4, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(62), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(63), "test", "", "aloism" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "EmployeeId", "Created", "Modified", "Password", "Token", "Username" },
                values: new object[] { 5, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(63), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(63), "test", "", "gertraudb" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "EmployeeId", "Created", "Modified", "Password", "Token", "Username" },
                values: new object[] { 6, new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(66), new DateTime(2024, 5, 23, 20, 59, 3, 936, DateTimeKind.Utc).AddTicks(66), "test", "", "alanf" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BirthDate",
                table: "Employees",
                column: "BirthDate");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_FirstName",
                table: "Employees",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LastName",
                table: "Employees",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeAddresses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
