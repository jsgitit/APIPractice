using CompanyWebApi.Contracts.Entities;
using Xunit;

namespace CompanyWebApi.Tests.UnitTests;
public class EmployeeAddressesTests
{
    [Fact]
    public void EmployeeAddressShouldBeAbleToSetWorkAddressType()
    {
        var address = new EmployeeAddress { EmployeeId = 1, AddressTypeId = AddressType.Work, Address = "Some Work Address" };
        Assert.Equal(AddressType.Work, address.AddressTypeId);
    }
}
