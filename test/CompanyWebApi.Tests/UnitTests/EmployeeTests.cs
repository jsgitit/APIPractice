using CompanyWebApi.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CompanyWebApi.Tests.UnitTests;
public class EmployeeTests
{
    [Fact]
    public void Employee_ShouldHaveMultipleAddresses()
    {
        var employee = new Employee
        {
            EmployeeId = 1,
            EmployeeAddresses = new List<EmployeeAddress>
            {
                new EmployeeAddress { EmployeeId = 1, AddressTypeId = AddressType.Work },
                new EmployeeAddress { EmployeeId = 1, AddressTypeId = AddressType.Residential }
            }
        };

        Assert.Equal(2, employee.EmployeeAddresses.Count);
    }


}
