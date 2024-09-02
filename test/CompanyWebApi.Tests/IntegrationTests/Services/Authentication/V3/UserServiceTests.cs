using CompanyWebApi.Services.Authentication.V3;
using CompanyWebApi.Tests.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.IntegrationTests.Services.Authentication.V3;

public class UserServiceTests : IClassFixture<WebApiTestFactory>
{
    private readonly IUserService _userService;

    public UserServiceTests(WebApiTestFactory factory)
    {
        _userService = factory.Services.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task CanAuthenticateUser()
    {
        var authenticatedUser = await _userService.AuthenticateAsync("johnw", "test");
        Assert.NotNull(authenticatedUser.Token);
        Assert.NotEmpty(authenticatedUser.Token);
    }
}
