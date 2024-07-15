using System;

namespace CompanyWebApi.Contracts.Dto.V3
{
    [Serializable]
    public class UserAuthenticateDto : UserDto
	{
        public string Token { get; set; }
	}
}
