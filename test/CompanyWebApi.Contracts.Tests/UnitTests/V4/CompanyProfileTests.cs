using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Contracts.MappingProfiles.V4;

namespace CompanyWebApi.Contracts.Tests.V4
{

    /// <summary>
    /// Suggested Unit Tests
    /// 
    /// 1. Basic Mapping Verification: Test that all properties of the Company 
    ///    entity are correctly mapped to the CompanyDto and CompanyFullDto.
    /// 2. Nested Object Mapping: Ensure that nested collections, 
    ///    such as Departments and Employees, are correctly mapped, and 
    ///    properties within these collections are accurately transferred.
    /// 3. Null and Default Values Handling: Verify that null and default values 
    ///    in the source objects are handled correctly and 
    ///    do not cause issues during the mapping process.
    /// 4. Complex Property Mapping: Even though the resolver for Username is straightforward, 
    ///    it’s still good to verify that this property is correctly handled 
    ///    in scenarios where User is null.
    /// 
    /// </summary>
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
                Name = "Test Company"
            };

            // Act
            var companyDto = _mapper.Map<CompanyDto>(company);

            // Assert
            Assert.Equal(company.CompanyId, companyDto.CompanyId);
            Assert.Equal(company.Name, companyDto.Name);
        }

        [Fact]
        public void CompanyToCompanyFullDto_ShouldMapCorrectly()
        {
            // Arrange
            var company = new Company
            {
                CompanyId = 999,
                Name = "This company should not be the one mapped",
                Departments = new List<Department>
                {
                    new Department
                    {
                        DepartmentId = 998,
                        Name = "This department should not be the one mapped",
                        Employees = new List<Employee>
                        {
                            new Employee
                            {
                                EmployeeId = 1,
                                FirstName = "John",
                                LastName = "Doe",
                                EmployeeAddresses = new List<EmployeeAddress>
                                {
                                    new EmployeeAddress
                                    {
                                        AddressTypeId = AddressType.Work,
                                        Address = "123 Main St"
                                    }
                                },
                                User = new User
                                {
                                    Username = "jdoe"
                                },
                                CompanyId = 998,
                                Company = new Company {CompanyId = 1, Name = "Test Company in the Employee object"},
                                DepartmentId = 1,
                                Department = new Department { DepartmentId = 1, Name = "HR"}
                            }
                        }
                    }
                }
            };

            // Act
            var companyFullDto = _mapper.Map<CompanyFullDto>(company);

            // Assert
            Assert.Equal(company.CompanyId, companyFullDto.CompanyId);
            Assert.Equal(company.Name, companyFullDto.Name);
            Assert.Single(companyFullDto.Employees);

            var employeeFullDto = companyFullDto.Employees.First();
            Assert.Equal(1, employeeFullDto.EmployeeId);
            Assert.Equal("John", employeeFullDto.FirstName);
            Assert.Equal("Doe", employeeFullDto.LastName);
            Assert.Equal(1, employeeFullDto.CompanyId);
            Assert.Equal("Test Company in the Employee object", employeeFullDto.Company);
            Assert.Equal(1, employeeFullDto.DepartmentId);
            Assert.Equal("HR", employeeFullDto.Department);
            Assert.Single(employeeFullDto.Addresses);
            Assert.Equal("123 Main St", employeeFullDto.Addresses.First().Address);
            Assert.Equal("jdoe", employeeFullDto.Username);
        }

        [Fact]
        public void CompanyToCompanyFullDto_ShouldHandleNullValues()
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
                                EmployeeId = 1,
                                FirstName = "John",
                                LastName = "Doe",
                                EmployeeAddresses = null,
                                User = null
                            }
                        }
                    }
                }
            };

            // Act
            var companyFullDto = _mapper.Map<CompanyFullDto>(company);

            // Assert
            var employeeDto = companyFullDto.Employees.First();
            Assert.NotNull(employeeDto.Addresses); // Should not be null, even if source is null
            Assert.Empty(employeeDto.Addresses); // Should be an empty list
            Assert.Equal(string.Empty, employeeDto.Username); // Should be empty string if User is null
        }

        [Fact]
        public void CompanyToCompanyFullDto_ShouldHandleEmptyCollections()
        {
            // Arrange
            var company = new Company
            {
                CompanyId = 1,
                Name = "Test Company",
                Departments = new List<Department>() // Empty departments list
            };

            // Act
            var companyFullDto = _mapper.Map<CompanyFullDto>(company);

            // Assert
            Assert.NotNull(companyFullDto.Employees);
            Assert.Empty(companyFullDto.Employees);
        }
    }
}
