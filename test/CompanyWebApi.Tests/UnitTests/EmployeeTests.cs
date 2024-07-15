using CompanyWebApi.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CompanyWebApi.Tests.UnitTests;
public class EmployeeTests
{
    [Fact]
    public void Employee_MustBeAbleToHaveMultipleAddresses()
    {
        var employee = new Employee
        {
            EmployeeId = 1,
            EmployeeAddresses = new List<EmployeeAddress>
            {
                new EmployeeAddress { EmployeeId = 1, AddressTypeId = AddressType.Work, Address = "123 Downtown Street Ste 123, Some Town, 99999" },
                new EmployeeAddress { EmployeeId = 1, AddressTypeId = AddressType.Residential, Address = "123 Some Street APT 123, Some Town, 99999" },
                new EmployeeAddress { EmployeeId = 1, AddressTypeId = AddressType.Mailing, Address = "123 Some Street APT 123, Some Town, 99999" }
            }
        };

        var addressList = employee.EmployeeAddresses.Select(x => $"{x.AddressTypeId}: {x.Address}").ToList();

        Assert.Equal(3, employee.EmployeeAddresses.Count);
    }
}
