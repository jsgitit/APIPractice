﻿// <auto-generated />
using System;
using CompanyWebApi.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CompanyWebApi.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240610003431_ChangeEmployeeAndEmployeeAddressEntities")]
    partial class ChangeEmployeeAndEmployeeAddressEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.29");

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies", (string)null);

                    b.HasData(
                        new
                        {
                            CompanyId = 1,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Company One"
                        },
                        new
                        {
                            CompanyId = 2,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Company Two"
                        },
                        new
                        {
                            CompanyId = 3,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Company Three"
                        });
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.HasKey("DepartmentId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Departments", (string)null);

                    b.HasData(
                        new
                        {
                            DepartmentId = 1,
                            CompanyId = 1,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Logistics"
                        },
                        new
                        {
                            DepartmentId = 2,
                            CompanyId = 1,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Administration"
                        },
                        new
                        {
                            DepartmentId = 3,
                            CompanyId = 1,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Development"
                        },
                        new
                        {
                            DepartmentId = 4,
                            CompanyId = 2,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Sales"
                        },
                        new
                        {
                            DepartmentId = 5,
                            CompanyId = 2,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Marketing"
                        },
                        new
                        {
                            DepartmentId = 6,
                            CompanyId = 3,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Customer support"
                        },
                        new
                        {
                            DepartmentId = 7,
                            CompanyId = 3,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Research and Development"
                        },
                        new
                        {
                            DepartmentId = 8,
                            CompanyId = 3,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Purchasing"
                        },
                        new
                        {
                            DepartmentId = 9,
                            CompanyId = 3,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Human Resource Management"
                        },
                        new
                        {
                            DepartmentId = 10,
                            CompanyId = 3,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Accounting and Finance"
                        },
                        new
                        {
                            DepartmentId = 11,
                            CompanyId = 3,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Production"
                        });
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeId");

                    b.HasIndex("BirthDate");

                    b.HasIndex("CompanyId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("FirstName");

                    b.HasIndex("LastName");

                    b.ToTable("Employees", (string)null);

                    b.HasData(
                        new
                        {
                            EmployeeId = 1,
                            BirthDate = new DateTime(1991, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CompanyId = 1,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DepartmentId = 1,
                            FirstName = "John",
                            LastName = "Whyne",
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            EmployeeId = 2,
                            BirthDate = new DateTime(1997, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CompanyId = 2,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DepartmentId = 4,
                            FirstName = "Mathias",
                            LastName = "Gernold",
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            EmployeeId = 3,
                            BirthDate = new DateTime(1955, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CompanyId = 3,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DepartmentId = 7,
                            FirstName = "Julia",
                            LastName = "Reynolds",
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            EmployeeId = 4,
                            BirthDate = new DateTime(1935, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CompanyId = 1,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DepartmentId = 2,
                            FirstName = "Alois",
                            LastName = "Mock",
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            EmployeeId = 5,
                            BirthDate = new DateTime(2001, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CompanyId = 2,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DepartmentId = 6,
                            FirstName = "Gertraud",
                            LastName = "Bochold",
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            EmployeeId = 6,
                            BirthDate = new DateTime(1984, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CompanyId = 2,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DepartmentId = 6,
                            FirstName = "Alan",
                            LastName = "Ford",
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        });
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.EmployeeAddress", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AddressTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeId", "AddressTypeId");

                    b.ToTable("EmployeeAddresses", (string)null);

                    b.HasData(
                        new
                        {
                            EmployeeId = 1,
                            AddressTypeId = 1,
                            Address = "Kentucky, USA",
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            EmployeeId = 2,
                            AddressTypeId = 1,
                            Address = "Berlin, Germany",
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            EmployeeId = 3,
                            AddressTypeId = 1,
                            Address = "Los Angeles, USA",
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            EmployeeId = 4,
                            AddressTypeId = 1,
                            Address = "Vienna, Austria",
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            EmployeeId = 5,
                            AddressTypeId = 1,
                            Address = "Cologne, Germany",
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            EmployeeId = 6,
                            AddressTypeId = 1,
                            Address = "Milano, Italy",
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        });
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.User", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            EmployeeId = 1,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Password = "test",
                            Token = "",
                            Username = "johnw"
                        },
                        new
                        {
                            EmployeeId = 2,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Password = "test",
                            Token = "",
                            Username = "mathiasg"
                        },
                        new
                        {
                            EmployeeId = 3,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Password = "test",
                            Token = "",
                            Username = "juliar"
                        },
                        new
                        {
                            EmployeeId = 4,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Password = "test",
                            Token = "",
                            Username = "aloism"
                        },
                        new
                        {
                            EmployeeId = 5,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Password = "test",
                            Token = "",
                            Username = "gertraudb"
                        },
                        new
                        {
                            EmployeeId = 6,
                            Created = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Modified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Password = "test",
                            Token = "",
                            Username = "alanf"
                        });
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.Department", b =>
                {
                    b.HasOne("CompanyWebApi.Contracts.Entities.Company", "Company")
                        .WithMany("Departments")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.Employee", b =>
                {
                    b.HasOne("CompanyWebApi.Contracts.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyWebApi.Contracts.Entities.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Company");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.EmployeeAddress", b =>
                {
                    b.HasOne("CompanyWebApi.Contracts.Entities.Employee", "Employee")
                        .WithMany("EmployeeAddresses")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.User", b =>
                {
                    b.HasOne("CompanyWebApi.Contracts.Entities.Employee", "Employee")
                        .WithOne("User")
                        .HasForeignKey("CompanyWebApi.Contracts.Entities.User", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.Company", b =>
                {
                    b.Navigation("Departments");
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("CompanyWebApi.Contracts.Entities.Employee", b =>
                {
                    b.Navigation("EmployeeAddresses");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
