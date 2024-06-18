using CompanyWebApi.Contracts.Dto;
using CompanyWebApi.Tests.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.IntegrationTests;
public class EmployeeAddressControllerTests : ControllerTestsBase
{
    private const string API_VERSION = "V2";
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
    public async Task CanCreateEmployeeAddress()
    {
        var newEmployeeAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 1,
            AddressTypeId = AddressType.Mailing,
            Address = "123 Mailing Address Street, Some Town, Oregon 97000",
        };
    }


    [Fact]
    public async Task CanCreateAndDeleteEmployeeAddress()
    {
        var newEmployeeAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 5,
            AddressTypeId = AddressType.Mailing,
            Address = "123 mailing address street, some unit, Seattle, WA."
        };
        var employeeAddress = await _httpClientHelper.PostAsync<EmployeeAddressCreateDto, EmployeeAddressDto>(_baseUrl + "create", newEmployeeAddress);
        Assert.Equal(newEmployeeAddress.Address, employeeAddress.Address);
        await _httpClientHelper.DeleteAsync(_baseUrl + $"{employeeAddress.EmployeeId}/{employeeAddress.AddressTypeId}");
    }

    [Fact]
    public async Task CanGetAllEmployeeAddresses()
    {

        var newUnknownAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 6,
            AddressTypeId = AddressType.Unknown,
            Address = "Unknown"
        };
        var employeeUnknownAddress = await _httpClientHelper.PostAsync<EmployeeAddressCreateDto, EmployeeAddressDto>(_baseUrl + "create", newUnknownAddress);

        var newMailingAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 6,
            AddressTypeId = AddressType.Mailing,
            Address = "123 mailing address street, some unit, Seattle, WA."
        };
        var employeeMailingAddress = await _httpClientHelper.PostAsync<EmployeeAddressCreateDto, EmployeeAddressDto>(_baseUrl + "create", newMailingAddress);

        var newResidentialAddress = new EmployeeAddressCreateDto
        {
            EmployeeId = 6,
            AddressTypeId = AddressType.Residential,
            Address = "Residential address"
        };
        var employeeResidentialAddress = await _httpClientHelper.PostAsync<EmployeeAddressCreateDto, EmployeeAddressDto>(_baseUrl + "create", newResidentialAddress);

        var employeesAddresses = await _httpClientHelper.GetAsync<List<EmployeeAddressDto>>(_baseUrl + "6");
        Assert.Equal(4, employeesAddresses.Count);

    }

    [Fact]
    public async Task CanGetSingleEmployeeAddressByType()
    {
        var employeeAddress = await _httpClientHelper.GetAsync<EmployeeAddressDto>(_baseUrl + "6/1");
        Assert.Equal(6, employeeAddress.EmployeeId);
        Assert.Equal(AddressType.Work, employeeAddress.AddressTypeId);
        Assert.NotNull(employeeAddress.Address);
    }

    [Fact]
    public async Task CanUpdateEmployeeAddress()
    {

        var newEmployeeAddress = new EmployeeAddressUpdateDto()
        {
            EmployeeId = 6,
            AddressTypeId = AddressType.Work,
            Address = "address updated"
        };

        // Update employee
        var updatedEmployee = await _httpClientHelper.PutAsync<EmployeeAddressUpdateDto, EmployeeAddressDto>(_baseUrl + "update", newEmployeeAddress);

        // address should be updated
        Assert.Equal("address updated", updatedEmployee.Address);
    }

}
