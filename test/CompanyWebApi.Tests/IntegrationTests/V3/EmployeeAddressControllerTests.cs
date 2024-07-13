using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Tests.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.IntegrationTests.V3;
public class EmployeeAddressControllerTests : ControllerTestsBase
{
    private const string API_VERSION = "V3.0";
    private readonly string _baseUrl;
    private readonly HttpClientHelper _httpClientHelper;

    public EmployeeAddressControllerTests(WebApiTestFactory factory)
        : base(factory)
    {
        _baseUrl = $"/api/{API_VERSION.ToLower()}/employeeAddresses/";
        _httpClientHelper = new HttpClientHelper(Client);
        _httpClientHelper.Client.SetFakeBearerToken((object)Token);
    }

    [Fact]
    public async Task CanCreateOneEmployeeAddress()
    {
        // Arrange
        var newEmployeeAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 1,
            AddressTypeId = AddressType.Mailing,
            Address = "123 Mailing Address Street, Some Town, Oregon 97000"
        };

        // Act
        var createdEmployeeAddress = await _httpClientHelper
            .PostAsync<EmployeeAddressCreateDto, EmployeeAddressDto>($"{_baseUrl}create", newEmployeeAddress);

        // Assert
        Assert.Equal(newEmployeeAddress.Address, createdEmployeeAddress.Address);
    }


    [Fact]
    public async Task CanCreateAndDeleteEmployeeAddress()
    {
        // Arrange
        var newEmployeeAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 5,
            AddressTypeId = AddressType.Mailing,
            Address = "123 mailing address street, some unit, Seattle, WA."
        };

        // Act
        var createdEmployeeAddress = await _httpClientHelper
            .PostAsync<EmployeeAddressCreateDto, EmployeeAddressDto>($"{_baseUrl}create", newEmployeeAddress);

        // Assert
        Assert.Equal(newEmployeeAddress.Address, createdEmployeeAddress.Address);

        // Cleanup
        await _httpClientHelper.DeleteAsync($"{_baseUrl}{createdEmployeeAddress.EmployeeId}/{createdEmployeeAddress.AddressTypeId}");
    }


    [Fact]
    public async Task CanGetAllEmployeeAddressesForOneEmployee()
    {
        // Arrange
        var newUnknownAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 6,
            AddressTypeId = AddressType.Unknown,
            Address = "Unknown"
        };
        var newMailingAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 6,
            AddressTypeId = AddressType.Mailing,
            Address = "123 mailing address street, some unit, Seattle, WA."
        };
        var newResidentialAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 6,
            AddressTypeId = AddressType.Residential,
            Address = "Residential address"
        };

        var employeeUnknownAddress = await _httpClientHelper
            .PostAsync<EmployeeAddressCreateDto, EmployeeAddressDto>($"{_baseUrl}create", newUnknownAddress);
        var employeeMailingAddress = await _httpClientHelper
            .PostAsync<EmployeeAddressCreateDto, EmployeeAddressDto>($"{_baseUrl}create", newMailingAddress);
        var employeeResidentialAddress = await _httpClientHelper
            .PostAsync<EmployeeAddressCreateDto, EmployeeAddressDto>($"{_baseUrl}create", newResidentialAddress);

        // Act
        var employeesAddresses = await _httpClientHelper
            .GetAsync<List<EmployeeAddressDto>>($"{_baseUrl}6");

        // Assert
        Assert.Equal(4, employeesAddresses.Count);

        // Cleanup
        await _httpClientHelper.DeleteAsync($"{_baseUrl}6/0");
        await _httpClientHelper.DeleteAsync($"{_baseUrl}6/2");
        await _httpClientHelper.DeleteAsync($"{_baseUrl}6/3");
    }


    [Fact]
    public async Task CanGetSingleEmployeeAddressByType()
    {
        // Arrange
        var employeeId = 6;
        var addressTypeId = AddressType.Work;

        // Act
        var employeeAddress = await _httpClientHelper
            .GetAsync<EmployeeAddressDto>($"{_baseUrl}{employeeId}/{addressTypeId}");

        // Assert
        Assert.Equal(employeeId, employeeAddress.EmployeeId);
        Assert.Equal(addressTypeId, employeeAddress.AddressTypeId);
        Assert.NotNull(employeeAddress.Address);
    }


    [Fact]
    public async Task CanUpdateSingleEmployeeAddress()
    {
        // Arrange
        var newEmployeeAddress = new EmployeeAddressUpdateDto
        {
            EmployeeId = 6,
            AddressTypeId = AddressType.Work,
            Address = "address updated"
        };

        // Act
        var updatedEmployee = await _httpClientHelper
            .PutAsync<EmployeeAddressUpdateDto, EmployeeAddressDto>($"{_baseUrl}update", newEmployeeAddress);

        // Assert
        Assert.Equal("address updated", updatedEmployee.Address);
    }


    [Fact]
    public async Task CanUpsertBatchOfEmployeeAddresses()
    {
        // Arrange
        var employeeAddresses = new List<EmployeeAddressUpdateDto>
        {
            new EmployeeAddressUpdateDto { EmployeeId = 5, AddressTypeId = AddressType.Work, Address = "work address change for emp 5" },
            new EmployeeAddressUpdateDto { EmployeeId = 5, AddressTypeId = AddressType.Residential, Address = "new residential address for emp 5" },
            new EmployeeAddressUpdateDto { EmployeeId = 6, AddressTypeId = AddressType.Work, Address = "work address change for emp 6" },
            new EmployeeAddressUpdateDto { EmployeeId = 6, AddressTypeId = AddressType.Unknown, Address = "new unknown address for emp 6" },
            new EmployeeAddressUpdateDto { EmployeeId = 6, AddressTypeId = AddressType.Residential, Address = "residential address change for emp 6" },
            new EmployeeAddressUpdateDto { EmployeeId = 6, AddressTypeId = AddressType.Mailing, Address = "mail address change for emp 6" }
        };

        // Act
        var putResponse = await _httpClientHelper.PutAsync<IList<EmployeeAddressUpdateDto>, HttpResponseMessage>(
            $"{_baseUrl}upsertBatch", employeeAddresses);

        // Assert
        Assert.Null(putResponse); // NoContent

        var empAddressesFor5 = await _httpClientHelper.GetAsync<List<EmployeeAddressDto>>($"{_baseUrl}5");
        var empAddressesFor6 = await _httpClientHelper.GetAsync<List<EmployeeAddressDto>>($"{_baseUrl}6");
        var updatedAddresses = empAddressesFor5.Concat(empAddressesFor6).ToList();

        Assert.Equal(6, updatedAddresses.Count);
        Assert.Equivalent(employeeAddresses, updatedAddresses, strict: false); // changed to false because updatedAddresses now have audit dates

        // Reset test data
        var oldWorkAddressFor5 = new EmployeeAddressUpdateDto
        { EmployeeId = 5, AddressTypeId = AddressType.Work, Address = "Cologne, Germany"};

        await _httpClientHelper.PutAsync<EmployeeAddressUpdateDto, EmployeeAddressDto>(
            $"{_baseUrl}update", oldWorkAddressFor5);

        var oldWorkAddressFor6 = new EmployeeAddressUpdateDto
        { EmployeeId = 6, AddressTypeId = AddressType.Work, Address = "Milano, Italy" };

        await _httpClientHelper.PutAsync<EmployeeAddressUpdateDto, EmployeeAddressDto>(
            $"{_baseUrl}update", oldWorkAddressFor6);

        await _httpClientHelper.DeleteAsync($"{_baseUrl}5/2");
        await _httpClientHelper.DeleteAsync($"{_baseUrl}6/0");
        await _httpClientHelper.DeleteAsync($"{_baseUrl}6/2");
        await _httpClientHelper.DeleteAsync($"{_baseUrl}6/3");
    }

    [Fact(Skip = "No ActionResult support in HttpClientHelper.cs")]
    public async Task CannotAddAddressForNonExistentEmployee()
    {
        // Arrange non-existent employee 9999
        var newUnknownAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 9999,
            AddressTypeId = AddressType.Unknown,
            Address = "Unknown"
        };

        // Act
        var response = await _httpClientHelper
            .PutAsync<EmployeeAddressCreateDto, HttpResponseMessage>($"{_baseUrl}create", newUnknownAddress);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact(Skip = "No ActionResult support in HttpClientHelper.cs")]
    public async Task CannotUpsertAddressForNonExistentEmployee()
    {
        // Arrange non-existent employee 9999


        var newUnknownAddresses = new List<EmployeeAddressUpdateDto>
        { 
            new EmployeeAddressUpdateDto { EmployeeId = 9999, AddressTypeId = AddressType.Unknown, Address = "Unknown" } 
        };

        // Act

        var response = await _httpClientHelper
            .PutAsync<IList<EmployeeAddressUpdateDto>, HttpResponseMessage>(
            $"{_baseUrl}upsertBatch", newUnknownAddresses);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
}
