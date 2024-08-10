using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Tests.Services;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.IntegrationTests.V4
{
    public class DepartmentsControllerTests : ControllerTestsBase
    {
        private const string API_VERSION = "V4";
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
        public async Task CanCreateAndDeleteDepartments()
        {
            var newDepartment = new DepartmentCreateDto
            {
                CompanyId = 1,
                Name = "Test Department"
            };
            var department = await _httpClientHelper.PostAsync<DepartmentCreateDto, DepartmentDto>(_baseUrl, newDepartment);
            Assert.Equal(newDepartment.Name, department.Name);
            await _httpClientHelper.DeleteAsync(_baseUrl + $"DeleteDepartmentById{API_VERSION}/{department.DepartmentId}");
        }

        [Fact]
        public async Task CanGetSingleDepartment()
        {
            var department = await _httpClientHelper.GetAsync<DepartmentDto>(_baseUrl + "3");
            Assert.Equal(3, department.DepartmentId);
            Assert.Equal("Development", department.Name);
        }

        [Fact]
        public async Task CanGetAllDepartments()
        {
            var departments = await _httpClientHelper.GetAsync<DepartmentListDto>(_baseUrl);
            Assert.Contains(departments.Departments, p => p.Name == "Logistics");
        }

        [Fact]
        public async Task CanGetSingleDepartmentWithFullDetails()
        {
            var department = await _httpClientHelper.GetAsync<DepartmentFullDto>(_baseUrl + "1/full");
            Assert.Equal(1, department.DepartmentId);
            Assert.Equal("Logistics", department.Name);
            Assert.NotNull(department.Employees);
            Assert.NotNull(department.Employees.First().Addresses);
            Assert.NotNull(department.Employees.First().Department);
        }

        [Fact]
        public async Task CanGetAllDepartmentsWithFullDetails()
        {
            var department = await _httpClientHelper.GetAsync<DepartmentFullListDto>(_baseUrl + "full");
            Assert.Contains(department.Departments, d => d.Name == "Development");
            Assert.NotNull(department.Departments.First().Employees);
            Assert.NotNull(department.Departments.First().Employees.First().Addresses);
            Assert.NotNull(department.Departments.First().Employees.First().Department);
        }

        [Fact]
        public async Task CanUpdateDepartment()
        {
            var departmentToUpdate = new DepartmentUpdateDto()
            {
                DepartmentId = 2,
                Name = "TEST DEPARTMENT"
            };
            var updatedDepartment = await _httpClientHelper.PutAsync<DepartmentUpdateDto, DepartmentDto>(_baseUrl, departmentToUpdate);
            Assert.Equal("TEST DEPARTMENT", updatedDepartment.Name);
        }
    }
}
