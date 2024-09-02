using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Tests.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.IntegrationTests.Controllers.V3
{
    public class DepartmentsControllerTests : ControllerTestsBase
    {
        private const string API_VERSION = "V3";
        private readonly string _baseUrl;
        private readonly HttpClientHelper _httpClientHelper;

        public DepartmentsControllerTests(WebApiTestFactory factory)
            : base(factory)
        {
            _baseUrl = $"/api/{API_VERSION.ToLower()}/departments/";
            _httpClientHelper = new HttpClientHelper(Client);
            _httpClientHelper.Client.SetFakeBearerToken((object)Token);
        }

        [Fact]
        public async Task CanCreateAndDelete()
        {
            var newDepartment = new DepartmentCreateDto
            {
                CompanyId = 1,
                Name = "Test Department"
            };
            var department = await _httpClientHelper.PostAsync<DepartmentCreateDto, DepartmentDto>(_baseUrl + "create", newDepartment);
            Assert.Equal(newDepartment.Name, department.Name);
            await _httpClientHelper.DeleteAsync(_baseUrl + $"DeleteDepartmentById{API_VERSION}/{department.DepartmentId}");
        }

        [Fact]
        public async Task CanGetAll()
        {
            var departments = await _httpClientHelper.GetAsync<List<DepartmentDto>>(_baseUrl + "getall");
            Assert.Contains(departments, p => p.Name == "Development");
        }

        [Fact]
        public async Task CanGetSingle()
        {
            var department = await _httpClientHelper.GetAsync<DepartmentDto>(_baseUrl + "3");
            Assert.Equal(3, department.DepartmentId);
            Assert.Equal("Development", department.Name);
        }

        [Fact]
        public async Task CanUpdate()
        {
            var departmentToUpdate = new DepartmentUpdateDto()
            {
                DepartmentId = 1,
                Name = "TEST DEPARTMENT"
            };
            var updatedDepartment = await _httpClientHelper.PutAsync<DepartmentUpdateDto, DepartmentDto>(_baseUrl + "update", departmentToUpdate);
            Assert.Equal("TEST DEPARTMENT", updatedDepartment.Name);
        }
    }
}
