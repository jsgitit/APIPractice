using CompanyWebApi.Contracts.Dto.V3;
using System.Threading.Tasks;

namespace CompanyWebApi.Services.Authentication.V3
{
	public interface IUserService
	{
		Task<UserAuthenticateDto> AuthenticateAsync (string username, string password);
	}
}
