﻿using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Persistence.Repositories;
using CompanyWebApi.Tests.Factories;
using CompanyWebApi.Tests.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.UnitTests
{
    [Collection("Sequential")]
    public class EmployeeRepositoryTests : IClassFixture<WebApiTestFactory>
    {
        private readonly ILogger _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeRepositoryTests(WebApiTestFactory factory)
        {
            _logger = factory.Services.GetRequiredService<ILogger<WebApiTestFactory>>();
            _employeeRepository = factory.Services.GetRequiredService<IEmployeeRepository>();
        }

        [Theory]
        [MemberData(nameof(EmployeeTestFactory.Employees), MemberType = typeof(EmployeeTestFactory))]
        public async Task CanAddEmployees(Employee employee)
        {
            var repoEmployee = await _employeeRepository.AddEmployeeAsync(employee);
            Assert.True(repoEmployee.EmployeeId > 0);
        }

        [Theory]
        [MemberData(nameof(EmployeeTestFactory.Employees), MemberType = typeof(EmployeeTestFactory))]
        public async Task IsAdult(Employee employee)
        {
            var repoEmployee = await _employeeRepository.AddEmployeeAsync(employee);
            var isAdult = EmployeeTestFactory.IsAdult(repoEmployee);
            Assert.True(isAdult);
        }

        [Fact]
        public async Task CanAdd()
        {
            _logger.LogInformation("CanAdd");
            var employee = new Employee
            {
                EmployeeId = 9999,
                FirstName = "TesterFirstName",
                LastName = "TesterLastName",
                BirthDate = new DateTime(2001, 12, 16),
                CompanyId = 1,
                DepartmentId = 1,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            var repoEmployee = await _employeeRepository.AddEmployeeAsync(employee);
            Assert.Equal("TesterLastName", repoEmployee.LastName);
        }

        [Fact]
        public async Task CanCount()
        {
            var numEmployees = await _employeeRepository.CountAsync();
            Assert.True(numEmployees > 0);
        }

        [Fact]
        public async Task CanDelete()
        {
            var employee = new Employee
            {
                EmployeeId = 99999,
                FirstName = "TesterFirstName",
                LastName = "TesterLastName",
                BirthDate = new DateTime(2001, 12, 16),
                CompanyId = 1,
                DepartmentId = 1,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            var repoEmployee = await _employeeRepository.AddEmployeeAsync(employee);
            _employeeRepository.Remove(repoEmployee);
            await _employeeRepository.SaveAsync();
            var deletedEmployee = await _employeeRepository.GetEmployeeAsync(employee.EmployeeId);
            Assert.Null(deletedEmployee);
        }

        [Fact]
        public async Task CanGetAllByPredicate()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            Assert.True(employees.Any());
        }

        [Fact]
        public async Task CanGetSingle()
        {
            var employee = await _employeeRepository.GetEmployeeAsync(cmp => cmp.FirstName.Equals("Alois") && cmp.LastName.Equals("Mock"));
            Assert.NotNull(employee);
        }

        [Fact]
        public async Task CanGetAll()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            Assert.True(employees.Any());
        }

        [Fact]
        public async Task CanUpdate()
        {
            var employee = new Employee
            {
                CompanyId = 1,
                DepartmentId = 3,
                EmployeeId = 3,
                FirstName = "Julia",
                LastName = "Reynolds Updated",
                BirthDate = new DateTime(1955, 12, 16),
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            await _employeeRepository.UpdateAsync(employee);
            await _employeeRepository.SaveAsync();
            Assert.Equal("Reynolds Updated", employee.LastName);
        }
    }
}
