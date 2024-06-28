using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Persistence.Repositories;
using CompanyWebApi.Tests.Factories;
using CompanyWebApi.Tests.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.UnitTests
{
    [Collection("Sequential")]
    public class EmployeeAddressRepositoryTests : IClassFixture<WebApiTestFactory>
    {
        private readonly ILogger<EmployeeAddressRepositoryTests> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeAddressRepository _employeeAddressRepository;

        public EmployeeAddressRepositoryTests(WebApiTestFactory factory)
        {
            _logger = factory.Services.GetRequiredService<ILogger<EmployeeAddressRepositoryTests>>();
            _employeeRepository = factory.Services.GetRequiredService<IEmployeeRepository>();
            _employeeAddressRepository = factory.Services.GetRequiredService<IEmployeeAddressRepository>();
        }

        [Theory]
        [MemberData(nameof(EmployeeAddressTestFactory.EmployeeAddresses), MemberType = typeof(EmployeeAddressTestFactory))]
        public async Task CanAddEmployeeAddress(EmployeeAddress address)
        {
            _logger.LogInformation("CanAddEmployeeAddress");
            //var repoEmployee = await _employeeRepository.GetEmployeeAsync(address.EmployeeId, tracking: true);
            //address.Employee = repoEmployee;
            var repoEmployeeAddress = await _employeeAddressRepository.AddEmployeeAddressAsync(address, tracking: true);
            Assert.NotNull(repoEmployeeAddress.Address);
        }

        [Fact]
        public async Task CanCount()
        {
            var numAddresses = await _employeeAddressRepository.CountAsync();
            Assert.True(numAddresses > 0);
        }

        [Fact]
        public async Task CanDelete()
        {
            var employeeAddress = new EmployeeAddress
            {
                EmployeeId = 1,
                AddressTypeId = AddressType.Work
                //Address = "Delete address",
                //Created = DateTime.UtcNow,
                //Modified = DateTime.UtcNow
            };

            _employeeAddressRepository.Remove(employeeAddress);
            await _employeeAddressRepository.SaveAsync();

            var deletedEmployeeAddress = await _employeeAddressRepository.GetEmployeeAddressAsync(employeeAddress.EmployeeId, employeeAddress.AddressTypeId);
            Assert.Null(deletedEmployeeAddress);
        }

        [Fact]
        public async Task CanGetAll()
        {
            var employeesAddresses = await _employeeAddressRepository.GetEmployeeAddressesAsync();
            Assert.True(employeesAddresses.Any());
        }

        [Fact]
        public async Task CanGetAllByPredicate()
        {
            var employeeAddresses = await _employeeAddressRepository
                .GetEmployeeAddressesAsync(ea => ea.Address.StartsWith("C"));
            Assert.True(employeeAddresses.Any());
        }

        [Fact]
        public async Task CanGetSingle()
        {
            var employeeAddress = await _employeeAddressRepository.GetEmployeeAddressAsync(ea => ea.Address.StartsWith("Kentucky"));
            Assert.NotNull(employeeAddress);
        }

        [Fact]
        public async Task CanUpdateSingleAddress()
        {
            var address = new EmployeeAddress
            {
                EmployeeId = 4,
                AddressTypeId = AddressType.Work, // AddressTypeId is immutable, since it's part of composite PK
                Address = "A new work address"
            };

            await _employeeAddressRepository.UpdateEmployeeAddressAsync(address);

            // Refresh the address entity from the context to reflect changes
            address = await _employeeAddressRepository.GetEmployeeAddressAsync(address.EmployeeId, address.AddressTypeId);

            Assert.Equal("A new work address", address.Address);
            Assert.Equal(AddressType.Work, address.AddressTypeId);
        }

        [Fact]
        public async Task CanUpdateAndInsertABatchOfAddresses()
        {
            // Arrange an update to an existing work address for Employee 2
            var employeeAddresses = (await _employeeAddressRepository
                .GetEmployeeAddressesAsync(ea => ea.EmployeeId == 2 && ea.AddressTypeId == AddressType.Work))
                .ToList(); // Convert to list to allow modifications

            var workAddress = employeeAddresses.FirstOrDefault();
            workAddress.Address = "123 changed work address";
            
            // Arrange a few new addresses for Employee 2
            employeeAddresses.Add(new EmployeeAddress
            {
                EmployeeId = 2,
                AddressTypeId = AddressType.Mailing,
                Address = "Initial Mailing Address"
            });
            employeeAddresses.Add(new EmployeeAddress
            {
                EmployeeId = 2,
                AddressTypeId = AddressType.Residential,
                Address = "Initial Residential Address"
            });

            await _employeeAddressRepository.UpsertEmployeeAddressesAsync(employeeAddresses); // "Upsert" -> is return type needed?
            await _employeeAddressRepository.SaveAsync();

            var updatedAddresses = (await _employeeAddressRepository
                .GetEmployeeAddressesAsync(ea => ea.EmployeeId == 2))
                .ToList();

            Assert.Equal("123 changed work address", updatedAddresses
                .FirstOrDefault(ea => ea.AddressTypeId == AddressType.Work)?.Address);
            Assert.True(updatedAddresses
                .FirstOrDefault(ea => ea.AddressTypeId == AddressType.Work)?.Modified > DateTime.UtcNow.AddMinutes(-1));
            Assert.True(updatedAddresses
                .FirstOrDefault(ea => ea.AddressTypeId == AddressType.Work)?.Created < DateTime.UtcNow.AddMinutes(-1));

            Assert.Equal("Initial Mailing Address", updatedAddresses
                .FirstOrDefault(ea => ea.AddressTypeId == AddressType.Mailing)?.Address);
            // Assert created/modified dates
            Assert.Equal("Initial Residential Address", updatedAddresses
                .FirstOrDefault(ea => ea.AddressTypeId == AddressType.Residential)?.Address);
            // Assert created/modified dates

        }


        //}
        // This CannotAddDuplicateEmployeeAddress() test is flaky
        // because it seems to cause other tests in this class to fail.
        // Failed test methods can run successfully when ran in isolation,
        // but not when combined with this test. There could be some test
        // cleanup which needs to occur within each test.
        // Or this test needs fixing.

        //[Fact]
        //public async Task CannotAddDuplicateEmployeeAddress()
        //{
        //    var existingEmployeeAddress = new EmployeeAddress
        //    {
        //        EmployeeId = 1,
        //        AddressTypeId = AddressType.Work,
        //        Address = "123 str"
        //    };

        //    var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        //    {
        //        await _employeeAddressRepository.AddEmployeeAddressAsync(existingEmployeeAddress);
        //    });
        //}
    }  
}
