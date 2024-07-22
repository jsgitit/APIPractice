using AutoMapper;
using CompanyWebApi.Contracts.Dto;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Contracts.MappingProfiles.V2_1;

namespace CompanyWebApi.Contracts.Tests.MappingProfiles.V2_1;

public class CompanyProfileTests
{
    private readonly IMapper _mapper;

    public CompanyProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CompanyProfile());
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void CompanyToCompanyDto_ShouldMapCorrectly()
    {
        // Arrange
        var company = new Company
        {
            CompanyId = 1,
            Name = "Test Company",
            Departments = new List<Department>
            {
                new Department
                {
                    Name = "HR",
                    Employees = new List<Employee>
                    {
                        new Employee
                        {
                            FirstName = "John",
                            LastName = "Doe",
                            EmployeeAddresses = new List<EmployeeAddress>
                            {
                                new EmployeeAddress
                                {
                                    AddressTypeId = AddressType.Work,
                                    Address = "123 Main St"
                                },
                                new EmployeeAddress
                                {
                                    AddressTypeId = AddressType.Residential,
                                    Address = "456 Home St"
                                }
                            },
                            User = new User
                            {
                                Username = "jdoe"
                            }
                        }
                    }
                }
            }
        };

        // Act
        var companyDto = _mapper.Map<CompanyDto>(company);

        // Assert
        Assert.Equal(company.CompanyId, companyDto.CompanyId);
        Assert.Equal(company.Name, companyDto.Name);
        Assert.Single(companyDto.Employees);
        Assert.Equal("John Doe, Address: 123 Main St, Department: HR, Username: jdoe", companyDto.Employees.First());
    }

    [Fact]
    public void CompaniesToCompanyDtos_ShouldMapCorrectly()
    {
        // Arrange
        var companies = new List<Company>
        {
            new Company
            {
                CompanyId = 1,
                Name = "Test Company 1",
                Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "HR",
                        Employees = new List<Employee>
                        {
                            new Employee
                            {
                                FirstName = "John",
                                LastName = "Doe",
                                EmployeeAddresses = new List<EmployeeAddress>
                                {
                                    new EmployeeAddress
                                    {
                                        AddressTypeId = AddressType.Work,
                                        Address = "123 Main St"
                                    },
                                    new EmployeeAddress
                                    {
                                        AddressTypeId = AddressType.Residential,
                                        Address = "456 Home St"
                                    }
                                },
                                User = new User
                                {
                                    Username = "jdoe"
                                }
                            }
                        }
                    }
                }
            },
            new Company
            {
                CompanyId = 2,
                Name = "Test Company 2",
                Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "IT",
                        Employees = new List<Employee>
                        {
                            new Employee
                            {
                                FirstName = "Jane",
                                LastName = "Smith",
                                EmployeeAddresses = new List<EmployeeAddress>
                                {
                                    new EmployeeAddress
                                    {
                                        AddressTypeId = AddressType.Work,
                                        Address = "789 Office St"
                                    }
                                },
                                User = new User
                                {
                                    Username = "jsmith"
                                }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var companyDtos = _mapper.Map<IList<CompanyDto>>(companies);

        // Assert
        Assert.Equal(companies.Count, companyDtos.Count);

        var firstCompanyDto = companyDtos.First();
        Assert.Equal(companies[0].CompanyId, firstCompanyDto.CompanyId);
        Assert.Equal(companies[0].Name, firstCompanyDto.Name);
        Assert.Single(firstCompanyDto.Employees);
        Assert.Equal("John Doe, Address: 123 Main St, Department: HR, Username: jdoe", firstCompanyDto.Employees.First());

        var secondCompanyDto = companyDtos.Last();
        Assert.Equal(companies[1].CompanyId, secondCompanyDto.CompanyId);
        Assert.Equal(companies[1].Name, secondCompanyDto.Name);
        Assert.Single(secondCompanyDto.Employees);
        Assert.Equal("Jane Smith, Address: 789 Office St, Department: IT, Username: jsmith", secondCompanyDto.Employees.First());
    }
}
