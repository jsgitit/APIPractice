using AutoMapper;
using CompanyWebApi.Contracts.Dto;
using CompanyWebApi.Contracts.Entities;

namespace CompanyWebApi.Contracts.MappingProfiles.V2_1.Tests;

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
        Assert.Equal(1, employeeAddress.EmployeeId);
        Assert.Equal(AddressType.Work, employeeAddress.AddressTypeId);
        Assert.Equal("123 Work St", employeeAddress.Address);
    }

    [Fact]
    public void EmployeeAddressCreateDtoListToEmployeeAddressList_ShouldMapCorrectly()
    {
        // Arrange
        var employeeAddressCreateDtoList = new List<EmployeeAddressCreateDto>
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
                Address = "456 Home Ave"
            }
        };

        // Act
        var employeeAddressList = _mapper.Map<IList<EmployeeAddress>>(employeeAddressCreateDtoList);

        // Assert
        Assert.NotNull(employeeAddressList);
        Assert.Equal(2, employeeAddressList.Count);

        var firstEmployeeAddress = employeeAddressList.First();
        Assert.Equal(1, firstEmployeeAddress.EmployeeId);
        Assert.Equal(AddressType.Work, firstEmployeeAddress.AddressTypeId);
        Assert.Equal("123 Work St", firstEmployeeAddress.Address);

        var secondEmployeeAddress = employeeAddressList.Last();
        Assert.Equal(2, secondEmployeeAddress.EmployeeId);
        Assert.Equal(AddressType.Residential, secondEmployeeAddress.AddressTypeId);
        Assert.Equal("456 Home Ave", secondEmployeeAddress.Address);
    }


    [Fact]
    public void EmployeeAddressToEmployeeAddressDto_ShouldMapCorrectly()
    {
        // Arrange
        var employeeAddress = new EmployeeAddress
        {
            EmployeeId = 1,
            AddressTypeId = AddressType.Work,
            Address = "123 Work St"
        };

        // Act
        var employeeAddressDto = _mapper.Map<EmployeeAddressDto>(employeeAddress);

        // Assert
        Assert.NotNull(employeeAddressDto);
        Assert.Equal(1, employeeAddressDto.EmployeeId);
        Assert.Equal(AddressType.Work, employeeAddressDto.AddressTypeId);
        Assert.Equal("123 Work St", employeeAddressDto.Address);
    }

    [Fact]
    public void EmployeeAddressListToEmployeeAddressDtoList_ShouldMapCorrectly()
    {
        // Arrange
        var employeeAddressList = new List<EmployeeAddress>
            {
                new EmployeeAddress
                {
                    EmployeeId = 1,
                    AddressTypeId = AddressType.Work,
                    Address = "123 Work St"
                },
                new EmployeeAddress
                {
                    EmployeeId = 2,
                    AddressTypeId = AddressType.Residential,
                    Address = "456 Home Ave"
                }
            };

        // Act
        var employeeAddressDtoList = _mapper.Map<IList<EmployeeAddressDto>>(employeeAddressList);

        // Assert
        Assert.NotNull(employeeAddressDtoList);
        Assert.Equal(2, employeeAddressDtoList.Count);

        var firstEmployeeAddressDto = employeeAddressDtoList.First();
        Assert.Equal(1, firstEmployeeAddressDto.EmployeeId);
        Assert.Equal(AddressType.Work, firstEmployeeAddressDto.AddressTypeId);
        Assert.Equal("123 Work St", firstEmployeeAddressDto.Address);

        var secondEmployeeAddressDto = employeeAddressDtoList.Last();
        Assert.Equal(2, secondEmployeeAddressDto.EmployeeId);
        Assert.Equal(AddressType.Residential, secondEmployeeAddressDto.AddressTypeId);
        Assert.Equal("456 Home Ave", secondEmployeeAddressDto.Address);
    }
}
