using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Converters.V3
{
	/// <summary>
	/// User to UserDto converter
	/// </summary>
	public class UserToDtoConverter : IConverter<User, UserDto>, IConverter<IList<User>, IList<UserDto>>
	{
		private readonly ILogger<UserToDtoConverter> _logger;

		public UserToDtoConverter(ILogger<UserToDtoConverter> logger)
		{
			_logger = logger;
		}

		public UserDto Convert(User user)
		{
			_logger.LogDebug("Convert");
			var userDto = new UserDto
			{
				EmployeeId = user.EmployeeId,
                FirstName = user.Employee.FirstName,
                LastName = user.Employee.LastName,
                Username = user.Username,
                Password = user.Password,
				Created = user.Created,
				Modified = user.Modified
			};
			return userDto;
		}

		public IList<UserDto> Convert(IList<User> users)
		{
			_logger.LogDebug("ConvertList");
			return users.Select(Convert).ToList();
		}
	}
}
