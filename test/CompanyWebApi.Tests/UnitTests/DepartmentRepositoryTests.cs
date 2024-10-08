﻿using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Persistence.Repositories;
using CompanyWebApi.Tests.Factories;
using CompanyWebApi.Tests.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.UnitTests
{
    [Collection("Sequential")]
    public class DepartmentRepositoryTests : IClassFixture<WebApiTestFactory>
    {
        private readonly ILogger _logger;
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentRepositoryTests(WebApiTestFactory factory)
        {
            _logger = factory.Services.GetRequiredService<ILogger<WebApiTestFactory>>();
            _departmentRepository = factory.Services.GetRequiredService<IDepartmentRepository>();
        }

        [Theory]
        [MemberData(nameof(DepartmentTestFactory.Departments), MemberType = typeof(DepartmentTestFactory))]
        public async Task CanAddDepartments(Department department)
        {
            var repoDepartment = await _departmentRepository.AddDepartmentAsync(department);
            Assert.True(repoDepartment.DepartmentId > 0);
        }

        [Fact]
        public async Task CanAdd()
        {
            _logger.LogInformation("CanAdd");
            var department = new Department
            {
                CompanyId = 1,
                //DepartmentId = 999,
                Name = "TEST DEPARTMENT"
            };
            var repoDepartment = await _departmentRepository.AddDepartmentAsync(department);
            Assert.Equal("TEST DEPARTMENT", repoDepartment.Name);
        }

        [Fact]
        public async Task CanAddRange()
        {
            var departments = new List<Department>
            {
                new()
                {
                    CompanyId = 1, DepartmentId = 1111, Name = "TEST1"
                },
                new()
                {
                    CompanyId = 1, DepartmentId = 2222, Name = "TEST2"
                },
                new()
                {
                    CompanyId = 1, DepartmentId = 3333, Name = "TEST3"
                }
            };
            await _departmentRepository.AddAsync(departments);
            await _departmentRepository.SaveAsync();
            Assert.True(departments.Count > 0);
        }

        [Fact]
        public async Task CanCount()
        {
            var nrCompanies = await _departmentRepository.CountAsync();
            Assert.True(nrCompanies > 0);
        }

        [Fact]
        public async Task CanDelete()
        {
            var department = new Department
            {
                CompanyId = 1,
                DepartmentId = 9999,
                Name = "TEST DEPARTMENT"
            };
            await _departmentRepository.AddDepartmentAsync(department);
            _departmentRepository.Remove(department);
            await _departmentRepository.SaveAsync();
            var repoDepartment = await _departmentRepository.GetDepartmentAsync(department.DepartmentId);
            Assert.Null(repoDepartment);
        }

        [Fact]
        public async Task CanGetByPredicate()
        {
            var department = await _departmentRepository.GetDepartmentAsync(dep => dep.Name.Equals("Development"));
            Assert.NotNull(department);
        }

        [Fact]
        public async Task CanGetAll()
        {
            var companies = await _departmentRepository.GetDepartmentsAsync();
            Assert.True(companies.Any());
        }

        [Fact]
        public async Task CanUpdate()
        {
            var department = new Department
            {
                DepartmentId = 1,
                CompanyId = 1, // added, since UpdateAsync() expects fully populated entity, else CompanyId=0, and fails FK constraint
                Name = "HR Updated"
            };
            await _departmentRepository.UpdateAsync(department);
            await _departmentRepository.SaveAsync();
            Assert.Equal("HR Updated", department.Name);
        }
    }
}
