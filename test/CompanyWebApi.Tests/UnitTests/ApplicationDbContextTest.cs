using CompanyWebApi.Persistence.DbContexts;
using CompanyWebApi.Tests.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace CompanyWebApi.Tests.UnitTests;
public class ApplicationDbContextTest : IClassFixture<WebApiTestFactory>
{
    private ApplicationDbContext _dbContext;

    public ApplicationDbContextTest(WebApiTestFactory factory)
    {
        _dbContext = factory.Services.GetRequiredService<ApplicationDbContext>();
    }
    [Fact]
    public void EmployeeShouldHaveManyEmployeeAddresses()
    {

        var employee = _dbContext.Employees.Include(e => e.EmployeeAddresses).FirstOrDefault();
        Assert.NotNull(employee);
        Assert.NotEmpty(employee.EmployeeAddresses);
    }

    [Fact]
    public void EmployeeAddressesShouldInitiallyBeWorkAddresses()
    {

        var address = _dbContext.EmployeeAddresses.First();
        Assert.Equal(AddressType.Work, address.AddressTypeId);
    }
}
