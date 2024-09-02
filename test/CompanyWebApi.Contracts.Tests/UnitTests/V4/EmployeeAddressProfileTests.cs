using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Contracts.MappingProfiles.V4;

namespace CompanyWebApi.Contracts.Tests.UnitTests.V4;

public class EmployeeAddressProfileTests
{
    private readonly IMapper _mapper;

    public EmployeeAddressProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<EmployeeAddressProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void EmployeeAddressToEmployeeAddressDto_ShouldMapCorrectly()
    {
        // Arrange
        var employeeAddress = new EmployeeAddress
        {
            EmployeeId = 1,
            AddressTypeId = AddressType.Work,
            Address = "123 Work St",
            Created = new DateTime(2020, 1, 1),
            Modified = new DateTime(2021, 1, 1)
        };

        // Act
        var employeeAddressDto = _mapper.Map<EmployeeAddressDto>(employeeAddress);

        // Assert
        Assert.NotNull(employeeAddressDto);
        Assert.Equal(employeeAddress.EmployeeId, employeeAddressDto.EmployeeId);
        Assert.Equal(employeeAddress.AddressTypeId, employeeAddressDto.AddressTypeId);
        Assert.Equal(employeeAddress.Address, employeeAddressDto.Address);
        Assert.Equal(employeeAddress.Created, employeeAddressDto.Created);
        Assert.Equal(employeeAddress.Modified, employeeAddressDto.Modified);
    }

    [Fact]
    public void EmployeeAddressesToEmployeeAddressDtos_ShouldMapCorrectly()
    {
        // Arrange
        var employeeAddresses = new List<EmployeeAddress>
        {
            new EmployeeAddress
            {
                EmployeeId = 1,
                AddressTypeId = AddressType.Work,
                Address = "123 Work St",
                Created = new DateTime(2020, 1, 1),
                Modified = new DateTime(2021, 1, 1)
            },
            new EmployeeAddress
            {
                EmployeeId = 2,
                AddressTypeId = AddressType.Residential,
                Address = "456 Residential Ave",
                Created = new DateTime(2019, 5, 10),
                Modified = new DateTime(2020, 5, 10)
            }
        };

        // Act
        var employeeAddressDtos = _mapper.Map<IList<EmployeeAddressDto>>(employeeAddresses);

        // Assert
        Assert.NotNull(employeeAddressDtos);
        Assert.Equal(2, employeeAddressDtos.Count);

        var firstEmployeeAddressDto = employeeAddressDtos.First();
        Assert.Equal(1, firstEmployeeAddressDto.EmployeeId);
        Assert.Equal(AddressType.Work, firstEmployeeAddressDto.AddressTypeId);
        Assert.Equal("123 Work St", firstEmployeeAddressDto.Address);

        var secondEmployeeAddressDto = employeeAddressDtos.Last();
        Assert.Equal(2, secondEmployeeAddressDto.EmployeeId);
        Assert.Equal(AddressType.Residential, secondEmployeeAddressDto.AddressTypeId);
        Assert.Equal("456 Residential Ave", secondEmployeeAddressDto.Address);
    }

    [Fact]
    public void EmployeeAddressUpdateDtoToEmployeeAddress_ShouldMapCorrectly()
    {
        // Arrange
        var employeeAddressUpdateDto = new EmployeeAddressUpdateDto
        {
            EmployeeId = 1,
            AddressTypeId = AddressType.Work,
            Address = "123 Work St"
        };

        // Act
        var employeeAddress = _mapper.Map<EmployeeAddress>(employeeAddressUpdateDto);

        // Assert
        Assert.NotNull(employeeAddress);
        Assert.Equal(employeeAddressUpdateDto.EmployeeId, employeeAddress.EmployeeId);
        Assert.Equal(employeeAddressUpdateDto.AddressTypeId, employeeAddress.AddressTypeId);
        Assert.Equal(employeeAddressUpdateDto.Address, employeeAddress.Address);
    }

    [Fact]
    public void EmployeeAddressUpdateDtosToEmployeeAddresses_ShouldMapCorrectly()
    {
        // Arrange
        var employeeAddressUpdateDtos = new List<EmployeeAddressUpdateDto>
        {
            new EmployeeAddressUpdateDto
            {
                EmployeeId = 1,
                AddressTypeId = AddressType.Work,
                Address = "123 Work St"
            },
            new EmployeeAddressUpdateDto
            {
                EmployeeId = 2,
                AddressTypeId = AddressType.Residential,
                Address = "456 Residential Ave"
            }
        };

        // Act
        var employeeAddresses = _mapper.Map<IList<EmployeeAddress>>(employeeAddressUpdateDtos);

        // Assert
        Assert.NotNull(employeeAddresses);
        Assert.Equal(2, employeeAddresses.Count);

        var firstEmployeeAddress = employeeAddresses.First();
        Assert.Equal(1, firstEmployeeAddress.EmployeeId);
        Assert.Equal(AddressType.Work, firstEmployeeAddress.AddressTypeId);
        Assert.Equal("123 Work St", firstEmployeeAddress.Address);

        var secondEmployeeAddress = employeeAddresses.Last();
        Assert.Equal(2, secondEmployeeAddress.EmployeeId);
        Assert.Equal(AddressType.Residential, secondEmployeeAddress.AddressTypeId);
        Assert.Equal("456 Residential Ave", secondEmployeeAddress.Address);
    }

    [Fact]
    public void EmployeeAddressCreateDtoToEmployeeAddress_ShouldMapCorrectly()
    {
        // Arrange
        var employeeAddressCreateDto = new EmployeeAddressCreateDto
        {
            EmployeeId = 1,
            AddressTypeId = AddressType.Work,
            Address = "123 Work St"
        };

        // Act
        var employeeAddress = _mapper.Map<EmployeeAddress>(employeeAddressCreateDto);

        // Assert
        Assert.NotNull(employeeAddress);
        Assert.Equal(employeeAddressCreateDto.EmployeeId, employeeAddress.EmployeeId);
        Assert.Equal(employeeAddressCreateDto.AddressTypeId, employeeAddress.AddressTypeId);
        Assert.Equal(employeeAddressCreateDto.Address, employeeAddress.Address);
    }

    [Fact]
    public void EmployeeAddressCreateDtosToEmployeeAddresses_ShouldMapCorrectly()
    {
        // Arrange
        var employeeAddressCreateDtos = new List<EmployeeAddressCreateDto>
        {
            new EmployeeAddressCreateDto
            {
                EmployeeId = 1,
                AddressTypeId = AddressType.Work,
                Address = "123 Work St"
            },
            new EmployeeAddressCreateDto
            {
                EmployeeId = 2,
                AddressTypeId = AddressType.Residential,
                Address = "456 Residential Ave"
            }
        };

        // Act
        var employeeAddresses = _mapper.Map<IList<EmployeeAddress>>(employeeAddressCreateDtos);

        // Assert
        Assert.NotNull(employeeAddresses);
        Assert.Equal(2, employeeAddresses.Count);

        var firstEmployeeAddress = employeeAddresses.First();
        Assert.Equal(1, firstEmployeeAddress.EmployeeId);
        Assert.Equal(AddressType.Work, firstEmployeeAddress.AddressTypeId);
        Assert.Equal("123 Work St", firstEmployeeAddress.Address);

        var secondEmployeeAddress = employeeAddresses.Last();
        Assert.Equal(2, secondEmployeeAddress.EmployeeId);
        Assert.Equal(AddressType.Residential, secondEmployeeAddress.AddressTypeId);
        Assert.Equal("456 Residential Ave", secondEmployeeAddress.Address);
    }
}
