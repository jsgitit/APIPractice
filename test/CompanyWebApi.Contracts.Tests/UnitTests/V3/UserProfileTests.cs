using AutoMapper;
using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Contracts.MappingProfiles.V3;

namespace CompanyWebApi.Contracts.Tests.UnitTests.V3;

public class UserProfileTests
{
    private readonly IMapper _mapper;

    public UserProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void UserToUserAuthenticateDto_ShouldMapCorrectly()
    {
        // Arrange
        var user = new User
        {
            EmployeeId = 1,
            Username = "jdoe",
            Password = "password",
            Token = "ey6blahblah",
            Employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe"
            }
        };

        // Act
        var userAuthenticateDto = _mapper.Map<UserAuthenticateDto>(user);

        // Assert
        Assert.NotNull(userAuthenticateDto);
        Assert.Equal(1, userAuthenticateDto.EmployeeId);
        Assert.Equal("jdoe", userAuthenticateDto.Username);
        Assert.Equal("password", userAuthenticateDto.Password);
        Assert.Equal("John", userAuthenticateDto.FirstName);
        Assert.Equal("Doe", userAuthenticateDto.LastName);
        Assert.Equal("ey6blahblah", userAuthenticateDto.Token);
    }

    [Fact]
    public void UserToUserDto_ShouldMapCorrectly()
    {
        // Arrange
        var user = new User
        {
            EmployeeId = 1,
            Username = "jdoe",
            Password = "password",
            Employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe"
            }
        };

        // Act
        var userDto = _mapper.Map<UserDto>(user);

        // Assert
        Assert.NotNull(userDto);
        Assert.Equal(1, userDto.EmployeeId);
        Assert.Equal("jdoe", userDto.Username);
        Assert.Equal("password", userDto.Password);
        Assert.Equal("John", userDto.FirstName);
        Assert.Equal("Doe", userDto.LastName);
    }

    [Fact]
    public void UsersToUserDtos_ShouldMapCorrectly()
    {
        // Arrange
        var users = new List<User>
        {
            new User
            {
                EmployeeId = 1,
                Username = "jdoe",
                Password = "password",
                Employee = new Employee
                {
                    FirstName = "John",
                    LastName = "Doe"
                }
            },
            new User
            {
                EmployeeId = 2,
                Username = "jsmith",
                Password = "password2",
                Employee = new Employee
                {
                    FirstName = "Jane",
                    LastName = "Smith"
                }
            }
        };

        // Act
        var userDtos = _mapper.Map<IList<UserDto>>(users);

        // Assert
        Assert.NotNull(userDtos);
        Assert.Equal(2, userDtos.Count);

        var firstUserDto = userDtos.First();
        Assert.Equal(1, firstUserDto.EmployeeId);
        Assert.Equal("jdoe", firstUserDto.Username);
        Assert.Equal("password", firstUserDto.Password);
        Assert.Equal("John", firstUserDto.FirstName);
        Assert.Equal("Doe", firstUserDto.LastName);

        var secondUserDto = userDtos.Last();
        Assert.Equal(2, secondUserDto.EmployeeId);
        Assert.Equal("jsmith", secondUserDto.Username);
        Assert.Equal("password2", secondUserDto.Password);
        Assert.Equal("Jane", secondUserDto.FirstName);
        Assert.Equal("Smith", secondUserDto.LastName);
    }
}
