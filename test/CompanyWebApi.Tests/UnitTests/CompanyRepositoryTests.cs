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
    public class CompanyRepositoryTests : IClassFixture<WebApiTestFactory>
    {
        private readonly ILogger _logger;
        private readonly ICompanyRepository _companyRepository;

        public CompanyRepositoryTests(WebApiTestFactory factory)
        {
            // Constructor is called for every test
            _logger = factory.Services.GetRequiredService<ILogger<WebApiTestFactory>>();
            _companyRepository = factory.Services.GetRequiredService<ICompanyRepository>();
        }

        [Theory]
        [MemberData(nameof(CompanyTestFactory.Companies), MemberType = typeof(CompanyTestFactory))]
        public async Task CanAddCompanies(Company company)
        {
            var repoCompany = await _companyRepository.AddCompanyAsync(company);
            Assert.True(repoCompany.CompanyId > 0);
        }

        [Fact]
        public async Task CanAdd()
        {
            _logger.LogInformation("CanAdd");
            var company = new Company
            {
                CompanyId = 999,
                Name = "New Company",
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            var repoCompany = await _companyRepository.AddCompanyAsync(company);
            Assert.Equal("New Company", repoCompany.Name);
        }

        [Fact]
        public async Task CanCount()
        {
            var nrCompanies = await _companyRepository.CountAsync();
            Assert.True(nrCompanies > 0);
        }

        [Fact]
        public async Task CanDelete()
        {
            var company = new Company
            {
                CompanyId = 9999,
                Name = "Delete Company",
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            await _companyRepository.AddCompanyAsync(company);
            _companyRepository.Remove(company);
            await _companyRepository.SaveAsync();
            var repoCompany = await _companyRepository.GetCompanyAsync(company.CompanyId);
            Assert.Null(repoCompany);
        }

        [Fact]
        public async Task CanGetAll()
        {
            var companies = await _companyRepository.GetCompaniesAsync();
            Assert.True(companies.Any());
        }

        [Fact]
        public async Task CanGetSingle()
        {
            var company = await _companyRepository.GetCompanyAsync(predicate: cmp => cmp.Name.Equals("Company One"));
            Assert.Equal("Company One", company.Name);
        }

        [Fact]
        public async Task CanUpdate()
        {
            var company = new Company
            {
                CompanyId = 3,
                Name = "Updated Test Company",
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            await _companyRepository.UpdateAsync(company);
            await _companyRepository.SaveAsync();
            Assert.Equal("Updated Test Company", company.Name);
        }
    }
}
