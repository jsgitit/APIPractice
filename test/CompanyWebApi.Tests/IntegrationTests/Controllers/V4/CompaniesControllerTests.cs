using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Tests.Services;
using Microsoft.AspNetCore.Components.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.IntegrationTests.Controllers.V4
{
    public class CompaniesControllerTests : ControllerTestsBase
    {
        private const string API_VERSION = "V4";
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
            var company = await _httpClientHelper.PostAsync<CompanyCreateDto, CompanyDto>(_baseUrl, newCompany);
            Assert.Equal("Test Company", company.Name);
            await _httpClientHelper.DeleteAsync(_baseUrl + $"{company.CompanyId}");
        }

        [Fact]
        public async Task CanGetSingleCompany()
        {
            var company = await _httpClientHelper.GetAsync<CompanyDto>(_baseUrl + "3");
            Assert.Equal(3, company.CompanyId);
            Assert.Equal("Company Three", company.Name);
        }

        [Fact]
        public async Task CanGetAllCompanies()
        {
            var companies = await _httpClientHelper.GetAsync<CompanyListDto>(_baseUrl);
            Assert.Contains(companies.Companies, p => p.Name == "Company One");
            Assert.Contains(companies.Companies, p => p.Name == "Company Two");
            Assert.Contains(companies.Companies, p => p.Name == "Company Three");
        }

        [Fact]
        public async Task CanGetSingleCompanyWithFullDetails()
        {
            var company = await _httpClientHelper.GetAsync<CompanyFullDto>(_baseUrl + "1/full");
            Assert.Equal(1, company.CompanyId);
            Assert.Equal("Company One", company.Name);
            Assert.NotNull(company.Employees);
            Assert.NotNull(company.Employees.First().Addresses);
            Assert.NotNull(company.Employees.First().Department);
        }

        [Fact]
        public async Task CanGetAllCompaniesWithFullDetails()
        {
            var companies = await _httpClientHelper.GetAsync<CompanyFullListDto>(_baseUrl + "full");
            Assert.Contains(companies.Companies, p => p.Name == "Company One");
            Assert.NotNull(companies.Companies.First().Employees);
            Assert.NotNull(companies.Companies.First().Employees.First().Addresses);
            Assert.NotNull(companies.Companies.First().Employees.First().Department);
        }

        [Fact]
        public async Task CanUpdateCompany()
        {
            // Note: Renaming a existing company while other tests are running caused a race condition 
            // for this test, so, I create a new company, then rename it to complete this test. 
            // Renaming an existing company caused other test methods like GetAllCompanies() to fail,
            // since the names would not match.

            var newCompany = new CompanyCreateDto
            {
                Name = "Test Company To Rename"
            };
            var company = await _httpClientHelper.PostAsync<CompanyCreateDto, CompanyDto>(_baseUrl, newCompany);
            var companyIdToRename = company.CompanyId;

            var companyToUpdate = new CompanyUpdateDto
            {
                CompanyId = companyIdToRename,
                Name = "Test Company Renamed"
            };
            var updatedCompany = await _httpClientHelper.PutAsync<CompanyUpdateDto, CompanyDto>(_baseUrl, companyToUpdate);
            Assert.Equal("Test Company Renamed", updatedCompany.Name);

            await _httpClientHelper.DeleteAsync(_baseUrl + $"{companyIdToRename}");
        }
    }
}
