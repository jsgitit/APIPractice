using CompanyWebApi.Contracts.Dto.V4;
using System.Threading.Tasks;

namespace CompanyWebApi.Services.Authentication.V4
{
	public interface IUserService
	{
		Task<UserAuthenticateDto> AuthenticateAsync (string username, string password);
	}
}
