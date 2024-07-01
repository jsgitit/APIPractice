using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Tests.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.IntegrationTests.V3
{
    public class CompaniesControllerTests : ControllerTestsBase
    {
        private const string API_VERSION = "V3";
        private readonly string _baseUrl;
        private readonly HttpClientHelper _httpClientHelper;

        public CompaniesControllerTests(WebApiTestFactory factory)
            : base(factory)
        {
            _baseUrl = $"/api/{API_VERSION.ToLower()}/companies/";
            _httpClientHelper = new HttpClientHelper(Client);
            _httpClientHelper.Client.SetFakeBearerToken((object)Token);
        }

        [Fact]
        public async Task CanCreateAndDeleteCompanies()
        {
            var newCompany = new CompanyCreateDto
            {
                Name = "Test Company"
            };
            var company = await _httpClientHelper.PostAsync<CompanyCreateDto, CompanyDto>(_baseUrl + "create", newCompany);
            Assert.Equal("Test Company", company.Name);
            await _httpClientHelper.DeleteAsync(_baseUrl + $"DeleteCompanyById{API_VERSION}/{company.CompanyId}");
        }

        [Fact]
        public async Task CanGetAllCompanies()
        {
            var companies = await _httpClientHelper.GetAsync<List<CompanyDto>>(_baseUrl + "getAll");
            Assert.Contains(companies, p => p.Name == "Company Two");
        }

        [Fact]
        public async Task CanGetCompany()
        {
            var company = await _httpClientHelper.GetAsync<CompanyDto>(_baseUrl + "3");
            Assert.Equal(3, company.CompanyId);
            Assert.Equal("Company Three", company.Name);
        }

        [Fact]
        public async Task CanUpdateCompany()
        {
            var companyToUpdate = new CompanyUpdateDto
            {
                CompanyId = 1,
                Name = "Test Company"
            };
            var updatedCompany = await _httpClientHelper.PutAsync<CompanyUpdateDto, CompanyDto>(_baseUrl + "update", companyToUpdate);
            Assert.Equal("Test Company", updatedCompany.Name);
        }
    }
}
