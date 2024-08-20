using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Contracts.MappingProfiles.V4;

namespace CompanyWebApi.Tests.MappingProfiles.V4;

public class EmployeeProfileTests
{
    private readonly IMapper _mapper;

    public EmployeeProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EmployeeProfile>());
        _mapper = config.CreateMapper();
        config.AssertConfigurationIsValid(); // missing mappings will be checked, use .Ignore() in profile if needed
    }

    [Fact]
    public void EmployeeToEmployeeDtoMapping_ShouldBeValid()
    {
        var employee = new Employee
        {
            EmployeeId = 1,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1980, 1, 1),
            CompanyId = 1,
            Company = new Company
            {
                CompanyId = 1,
                Name = "Acme Corp"
            },
            DepartmentId = 1,
            Department = new Department
            {
                DepartmentId = 1,
                Name = "HR"
            },
            EmployeeAddresses = new List<EmployeeAddress>
            {
                new EmployeeAddress
                {
                    EmployeeId = 1,
                    AddressTypeId = AddressType.Work,
                    Address = "123 Main St"
                }
            },
            User = new User
            {
                Username = "johndoe"
            },
            Created = DateTime.Now,
            Modified = DateTime.Now
        };

        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        Assert.Equal(employee.EmployeeId, employeeDto.EmployeeId);
        Assert.Equal(employee.FirstName, employeeDto.FirstName);
        Assert.Equal(employee.LastName, employeeDto.LastName);
        Assert.Equal(employee.BirthDate, employeeDto.BirthDate);
        Assert.Equal(employee.CompanyId, employeeDto.CompanyId);
        Assert.Equal(employee.Company.Name, employeeDto.Company);
        Assert.Equal(employee.DepartmentId, employeeDto.DepartmentId);
        Assert.Equal(employee.Department.Name, employeeDto.Department);
        Assert.Equal(employee.Created, employeeDto.Created);
        Assert.Equal(employee.Modified, employeeDto.Modified);
    }

    [Fact]
    public void EmployeeToEmployeeFullDtoMapping_ShouldBeValid()
    {
        var employee = new Employee
        {
            EmployeeId = 1,
            FirstName = "Jane",
            LastName = "Doe",
            BirthDate = new DateTime(1985, 6, 15),
            CompanyId = 2,
            Company = new Company
            {
                CompanyId = 2,
                Name = "Tech Corp"
            },
            DepartmentId = 2,
            Department = new Department
            {
                DepartmentId = 2,
                Name = "IT",
            },
            EmployeeAddresses = new List<EmployeeAddress>
            {
                new EmployeeAddress
                {
                    EmployeeId = 1,
                    AddressTypeId = AddressType.Residential,
                    Address = "456 Elm St"
                }
            },
            User = new User
            {
                Username = "janedoe"
            },
            Created = DateTime.Now,
            Modified = DateTime.Now
        };

        var employeeFullDto = _mapper.Map<EmployeeFullDto>(employee);

        Assert.Equal(employee.EmployeeId, employeeFullDto.EmployeeId);
        Assert.Equal(employee.FirstName, employeeFullDto.FirstName);
        Assert.Equal(employee.LastName, employeeFullDto.LastName);
        Assert.Equal(employee.BirthDate, employeeFullDto.BirthDate);
        Assert.Equal(employee.Age, employeeFullDto.Age);
        Assert.Equal(employee.User.Username, employeeFullDto.Username);
        Assert.Equal(employee.CompanyId, employeeFullDto.CompanyId);
        Assert.Equal(employee.Company.Name, employeeFullDto.Company);
        Assert.Equal(employee.DepartmentId, employeeFullDto.DepartmentId);
        Assert.Equal(employee.Department.Name, employeeFullDto.Department);
        Assert.Single(employeeFullDto.Addresses);
        Assert.Equal(employee.EmployeeAddresses.First().AddressTypeId, employeeFullDto.Addresses.First().AddressTypeId);
        Assert.Equal(employee.EmployeeAddresses.First().Address, employeeFullDto.Addresses.First().Address);
        Assert.Equal(employee.Created, employeeFullDto.Created);
        Assert.Equal(employee.Modified, employeeFullDto.Modified);
    }

    [Fact]
    public void EmployeeCreateDtoToEmployeeMapping_ShouldBeValid()
    {
        var employeeCreateDto = new EmployeeCreateDto
        {
            FirstName = "Alice",
            LastName = "Smith",
            BirthDate = new DateTime(1990, 3, 22),
            CompanyId = 3,
            DepartmentId = 3,
            Addresses = new List<EmployeeAddressCreateWithoutEmployeeIdDto>
            {
                new EmployeeAddressCreateWithoutEmployeeIdDto
                {
                    AddressTypeId = AddressType.Residential,
                    Address = "789 Maple St"
                }
            },
            Username = "alicesmith",
            Password = "password123"
        };

        var employee = _mapper.Map<Employee>(employeeCreateDto);

        Assert.Equal(employeeCreateDto.FirstName, employee.FirstName);
        Assert.Equal(employeeCreateDto.LastName, employee.LastName);
        Assert.Equal(employeeCreateDto.BirthDate, employee.BirthDate);
        Assert.Equal(employeeCreateDto.CompanyId, employee.CompanyId);
        Assert.Equal(employeeCreateDto.DepartmentId, employee.DepartmentId);
        Assert.Equal(employeeCreateDto.Addresses.Count, employee.EmployeeAddresses.Count);
        Assert.Equal(employeeCreateDto.Addresses.First().AddressTypeId, employee.EmployeeAddresses.First().AddressTypeId);
        Assert.Equal(employeeCreateDto.Addresses.First().Address, employee.EmployeeAddresses.First().Address);
        Assert.Equal(employeeCreateDto.Username, employee.User.Username);
        Assert.Equal(employeeCreateDto.Password, employee.User.Password);
    }

    [Fact]
    public void EmployeeAddressToEmployeeAddressDtoMapping_ShouldBeValid()
    {
        var employeeAddress = new EmployeeAddress
        {
            EmployeeId = 1,
            AddressTypeId = AddressType.Work,
            Address = "123 Main St"
        };

        var employeeAddressDto = _mapper.Map<EmployeeAddressDto>(employeeAddress);

        Assert.Equal(employeeAddress.EmployeeId, employeeAddressDto.EmployeeId);
        Assert.Equal(employeeAddress.AddressTypeId, employeeAddressDto.AddressTypeId);
        Assert.Equal(employeeAddress.Address, employeeAddressDto.Address);
    }

    [Fact]
    public void EmployeeAddressUpdateDtoToEmployeeAddressMapping_ShouldBeValid()
    {
        var employeeAddressUpdateDto = new EmployeeAddressUpdateDto
        {
            EmployeeId = 1,
            AddressTypeId = AddressType.Work,
            Address = "789 Oak St"
        };

        var employeeAddress = _mapper.Map<EmployeeAddress>(employeeAddressUpdateDto);

        Assert.Equal(employeeAddressUpdateDto.EmployeeId, employeeAddress.EmployeeId);
        Assert.Equal(employeeAddressUpdateDto.AddressTypeId, employeeAddress.AddressTypeId);
        Assert.Equal(employeeAddressUpdateDto.Address, employeeAddress.Address);
    }
}
