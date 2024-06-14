using CompanyWebApi.Contracts.Entities;
using Xunit;

namespace CompanyWebApi.Tests.Factories;

/// <summary>
/// Employee Address test factory
/// </summary>
public static class EmployeeAddressTestFactory
{
    /// <summary>
    /// Generate test employee address data
    /// </summary>
    public static TheoryData<EmployeeAddress> EmployeeAddresses =>
        new()
        {
                new EmployeeAddress
                {
                    EmployeeId = 1,
                    AddressTypeId = AddressType.Residential,
                    Address = "123 Residential St, Some Town 97000"
                },
                new EmployeeAddress
                {
                    EmployeeId = 2,
                    AddressTypeId = AddressType.Unknown,
                    Address = "456 Unknown St, Some Town 97000"
                },
                new EmployeeAddress
                {
                    EmployeeId = 3,
                    AddressTypeId = AddressType.Mailing,
                    Address = "789 Mailing St, Some Town 97000"
                }
        };
}
