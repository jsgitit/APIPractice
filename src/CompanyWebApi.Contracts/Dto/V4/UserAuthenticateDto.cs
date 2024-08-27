using System;

namespace CompanyWebApi.Contracts.Dto.V4;

[Serializable]
public class UserAuthenticateDto : UserDto
{
    public string Token { get; set; }
}
