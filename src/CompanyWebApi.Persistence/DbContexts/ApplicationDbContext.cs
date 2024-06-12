using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Contracts.Entities.Base;
using CompanyWebApi.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CompanyWebApi.Persistence.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        DateTime _fixedCreatedDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime _fixedModifiedDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeAddress> EmployeeAddresses { get; set; }

        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            TrackChanges();
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create EF entities and relations
            _ = new CompanyConfiguration(modelBuilder.Entity<Company>());
            _ = new EmployeeConfiguration(modelBuilder.Entity<Employee>());
            _ = new EmployeeAddressConfiguration(modelBuilder.Entity<EmployeeAddress>());
            _ = new DepartmentConfiguration(modelBuilder.Entity<Department>());
            _ = new UserConfiguration(modelBuilder.Entity<User>());

            // Add initial seed data for entities

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    CompanyId = 1,
                    Name = "Company One",
                    Created = _fixedCreatedDate,
                    Modified = _fixedModifiedDate
                },
                new Company
                {
                    CompanyId = 2,
                    Name = "Company Two",
                    Created = _fixedCreatedDate,
                    Modified = _fixedModifiedDate
                },
                new Company
                {
                    CompanyId = 3,
                    Name = "Company Three",
                    Created = _fixedCreatedDate,
                    Modified = _fixedModifiedDate
                });

            modelBuilder.Entity<Department>().HasData(
                new Department { CompanyId = 1, DepartmentId = 1, Name = "Logistics", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new Department { CompanyId = 1, DepartmentId = 2, Name = "Administration", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new Department { CompanyId = 1, DepartmentId = 3, Name = "Development", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new Department { CompanyId = 2, DepartmentId = 4, Name = "Sales", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new Department { CompanyId = 2, DepartmentId = 5, Name = "Marketing", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new Department { CompanyId = 3, DepartmentId = 6, Name = "Customer support", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new Department { CompanyId = 3, DepartmentId = 7, Name = "Research and Development", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new Department { CompanyId = 3, DepartmentId = 8, Name = "Purchasing", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new Department { CompanyId = 3, DepartmentId = 9, Name = "Human Resource Management", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new Department { CompanyId = 3, DepartmentId = 10, Name = "Accounting and Finance", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new Department { CompanyId = 3, DepartmentId = 11, Name = "Production", Created = _fixedCreatedDate, Modified = _fixedModifiedDate });

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    CompanyId = 1,
                    DepartmentId = 1,
                    EmployeeId = 1,
                    FirstName = "John",
                    LastName = "Whyne",
                    BirthDate = new DateTime(1991, 8, 7),
                    Created = _fixedCreatedDate,
                    Modified = _fixedModifiedDate
                },
                new Employee
                {
                    CompanyId = 2,
                    DepartmentId = 4,
                    EmployeeId = 2,
                    FirstName = "Mathias",
                    LastName = "Gernold",
                    BirthDate = new DateTime(1997, 10, 12),
                    Created = _fixedCreatedDate,
                    Modified = _fixedModifiedDate
                },
                new Employee
                {
                    CompanyId = 3,
                    DepartmentId = 7,
                    EmployeeId = 3,
                    FirstName = "Julia",
                    LastName = "Reynolds",
                    BirthDate = new DateTime(1955, 12, 16),
                    Created = _fixedCreatedDate,
                    Modified = _fixedModifiedDate
                },
                new Employee
                {
                    CompanyId = 1,
                    DepartmentId = 2,
                    EmployeeId = 4,
                    FirstName = "Alois",
                    LastName = "Mock",
                    BirthDate = new DateTime(1935, 2, 9),
                    Created = _fixedCreatedDate,
                    Modified = _fixedModifiedDate
                },
                new Employee
                {
                    CompanyId = 2,
                    DepartmentId = 6,
                    EmployeeId = 5,
                    FirstName = "Gertraud",
                    LastName = "Bochold",
                    BirthDate = new DateTime(2001, 3, 4),
                    Created = _fixedCreatedDate,
                    Modified = _fixedModifiedDate
                }
                ,
                new Employee
                {
                    CompanyId = 2,
                    DepartmentId = 6,
                    EmployeeId = 6,
                    FirstName = "Alan",
                    LastName = "Ford",
                    BirthDate = new DateTime(1984, 6, 15),
                    Created = _fixedCreatedDate,
                    Modified = _fixedModifiedDate
                });

            modelBuilder.Entity<EmployeeAddress>().HasData(
                new EmployeeAddress { EmployeeId = 1, Address = "Kentucky, USA", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new EmployeeAddress { EmployeeId = 2, Address = "Berlin, Germany", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new EmployeeAddress { EmployeeId = 3, Address = "Los Angeles, USA", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new EmployeeAddress { EmployeeId = 4, Address = "Vienna, Austria", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new EmployeeAddress { EmployeeId = 5, Address = "Cologne, Germany", Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new EmployeeAddress { EmployeeId = 6, Address = "Milano, Italy", Created = _fixedCreatedDate, Modified = _fixedModifiedDate });

            modelBuilder.Entity<User>().HasData(
                new User { EmployeeId = 1, Username = "johnw", Password = "test", Token = string.Empty, Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new User { EmployeeId = 2, Username = "mathiasg", Password = "test", Token = string.Empty, Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new User { EmployeeId = 3, Username = "juliar", Password = "test", Token = string.Empty, Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new User { EmployeeId = 4, Username = "aloism", Password = "test", Token = string.Empty, Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new User { EmployeeId = 5, Username = "gertraudb", Password = "test", Token = string.Empty, Created = _fixedCreatedDate, Modified = _fixedModifiedDate },
                new User { EmployeeId = 6, Username = "alanf", Password = "test", Token = string.Empty, Created = _fixedCreatedDate, Modified = _fixedModifiedDate });
        }

        /// <summary>
        /// Automatic change tracking
        /// </summary>
        private void TrackChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(x => x.Entity is IBaseAuditEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((IBaseAuditEntity)entry.Entity).Created = DateTime.UtcNow;
                }
                ((IBaseAuditEntity)entry.Entity).Modified = DateTime.UtcNow;
            }
        }
    }
}
