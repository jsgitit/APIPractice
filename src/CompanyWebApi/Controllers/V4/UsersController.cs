using Asp.Versioning;
using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Controllers.Base;
using CompanyWebApi.Persistence.Repositories.Factory;
using CompanyWebApi.Services.Authentication.V4;
using CompanyWebApi.Services.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CompanyWebApi.Controllers.V4;

[ApiAuthorization]
[ApiController]
[ApiVersion("4.0")]
[Produces("application/json")]
[EnableCors("EnableCORS")]
[ServiceFilter(typeof(ValidModelStateAsyncActionFilter))]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : BaseController<UsersController>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UsersController(IUserService userService,
        IRepositoryFactory repositoryFactory,
        IMapper mapper)
    {
        _userService = userService;
        _repositoryFactory = repositoryFactory;
        _mapper = mapper;
    }

    /// <summary>
    /// Add a new user
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///       "employeeId": 1,
    ///       "username": "alanf",
    ///       "password": "tntgroup!129"
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 1
    ///       "firstName": "Alan",
    ///       "lastName": "Ford",
    ///       "username": "alanf",
    ///       "password": "tntgroup!129",
    ///       "created": "2024-07-13T16:57:38.135Z",
    ///       "modified": "2024-07-13T16:57:38.135Z"
    ///     }
    /// </remarks>
    /// <param name="user">UserCreateDto model</param>
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(UserDto), Description = "Returns a new user")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The user with EmployeeId {user.EmployeeId} already exists")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee with {EmployeeId} was not found")]
    [HttpPost(Name = nameof(CreateUserAsync))]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateDto user)
    {
        Logger.LogDebug(nameof(CreateUserAsync));
        if (await _repositoryFactory.UserRepository.ExistsAsync(u => u.EmployeeId == user.EmployeeId).ConfigureAwait(false))
        {
            return BadRequest(new { message = $"The User with EmployeeId {user.EmployeeId} already exists" });
        }
        if (!await _repositoryFactory.EmployeeRepository.ExistsAsync(u => u.EmployeeId == user.EmployeeId).ConfigureAwait(false))
        {
            return NotFound(new { message = $"The Employee with EmployeeId {user.EmployeeId} was not found" });
        }

        var newUser = new User
        {
            EmployeeId = user.EmployeeId,
            Username = user.Username,
            Password = user.Password
        };

        var repoUser = await _repositoryFactory.UserRepository.AddUserAsync(newUser).ConfigureAwait(false);
        var result = _mapper.Map<UserDto>(repoUser);
        var createdResult = new ObjectResult(result)
        {
            StatusCode = StatusCodes.Status201Created
        };
        return createdResult;
    }

    /// <summary>
    /// Get a user 
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v4/users/alanf
    ///
    /// Sample response body:
    ///  
    ///      {
    ///        "employeeId": 8,
    ///        "firstName": "Alan",
    ///        "lastName": "Ford",
    ///        "username": "alanf",
    ///        "password": "tntgroup!129",
    ///        "created": "2024-07-13T16:57:38.135Z",
    ///        "modified": "2024-07-13T16:57:38.135Z"
    ///      }
    /// </remarks>
    /// <param name="userName"></param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IList<UserDto>), Description = "Return list of users")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The users were not found")]
    [HttpGet("{userName}", Name = nameof(GetUserByNameAsync))]
    public async Task<ActionResult<IList<UserDto>>> GetUserByNameAsync(string userName)
    {
        Logger.LogDebug(nameof(GetUserByNameAsync));
        var users = await _repositoryFactory.UserRepository.GetUsersByUsernameAsync(userName).ConfigureAwait(false);
        if (!users.Any())
        {
            return NotFound(new { message = $"The users with {userName} were not found" });
        }
        var usersDto = _mapper.Map<IList<UserDto>>(users);
        return Ok(usersDto);
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/v4/users
    ///
    /// Sample response body:
    /// 
    ///     [
    ///       {
    ///         "employeeId": 1,
    ///         "firstName": "Carl",
    ///         "lastName": "Weiss",
    ///         "username": "johnw",
    ///         "password": "test",
    ///         "created": "2024-07-13T16:57:38.135Z",
    ///         "modified": "2024-07-13T16:57:38.135Z"
    ///       },
    ///       {
    ///         "employeeId": 2,
    ///         "firstName": "Mathias",
    ///         "lastName": "Gernold",
    ///         "username": "mathiasg",
    ///         "password": "test",
    ///         "created": "2024-07-13T16:57:38.135Z",
    ///         "modified": "2024-07-13T16:57:38.135Z"
    ///       },
    ///       {
    ///         "employeeId": 3,
    ///         "firstName": "Julia",
    ///         "lastName": "Reynolds",
    ///         "username": "juliar",
    ///         "password": "test",
    ///         "created": "2024-07-13T16:57:38.135Z",
    ///         "modified": "2024-07-13T16:57:38.135Z"
    ///       }
    ///     ]
    /// </remarks>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IList<UserDto>), Description = "Return list of users")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The users list is empty")]
    [HttpGet(Name = nameof(GetAllUsersAsync))]
    public async Task<ActionResult<IList<UserDto>>> GetAllUsersAsync()
    {
        Logger.LogDebug(nameof(GetAllUsersAsync));
        var users = await _repositoryFactory.UserRepository.GetUsersAsync().ConfigureAwait(false);
        if (!users.Any())
        {
            return NotFound(new { message = "The users list is empty" });
        }
        var usersDto = _mapper.Map<IList<UserDto>>(users);
        return Ok(usersDto); // TODO: fix top level array, by introducing wrapper UserListDto.cs
    }

    /// <summary>
    /// Update user
    /// </summary>
    /// <remarks>
    /// Sample request body:
    ///
    ///     {
    ///       "employeeId": 1,
    ///       "username": "alanf",
    ///       "password": "new!pass"
    ///     }
    /// 
    /// Sample response body:
    /// 
    ///     {
    ///       "employeeId": 1,
    ///       "firstName": "Carl",
    ///       "lastName": "Weiss",
    ///       "username": "alanf",
    ///       "password": "new!pass",
    ///       "created": "2024-07-13T16:57:38.135Z",
    ///       "modified": "2024-07-13T16:57:38.135Z"
    ///     }
    /// </remarks>
    /// <param name="userToUpdate"><see cref="UserUpdateDto"/></param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDto), Description = "Return updated user")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found")]
    [HttpPut(Name = nameof(UpdateUserAsync))]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UserUpdateDto userToUpdate)
    {
        Logger.LogDebug(nameof(UpdateUserAsync));
        var repoUser = await _repositoryFactory.UserRepository.GetUserAsync(userToUpdate.EmployeeId).ConfigureAwait(false);
        if (repoUser == null)
        {
            return NotFound(new { message = "The user was not found" });
        }

        repoUser.Username = userToUpdate.Username;
        repoUser.Password = userToUpdate.Password;

        await _repositoryFactory.UserRepository.UpdateAsync(repoUser);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);


        var updatedUser = await _repositoryFactory.UserRepository.GetUserAsync(userToUpdate.EmployeeId).ConfigureAwait(false);
        var userDto = _mapper.Map<UserDto>(updatedUser);
        return Ok(userDto);
    }
    
    /// <summary>
    /// Deletes a user by user name
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/v4/users/username
    ///
    /// Sample response body:
    ///     
    ///     Code 204 No Content
    /// 
    /// </remarks>
    /// <param name="userName">User name</param>
    [SwaggerResponse(StatusCodes.Status204NoContent, Description = "User was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No user was found")]
    [HttpDelete("{userName}", Name = nameof(DeleteUserByNameAsync))]
    public async Task<ActionResult> DeleteUserByNameAsync([Required] string userName)
    {
        Logger.LogDebug(nameof(DeleteUserByNameAsync));
        var users = await _repositoryFactory.UserRepository.GetUsersByUsernameAsync(userName).ConfigureAwait(false);
        if (!users.Any())
        {
            return NotFound(new { message = $"The users with Username {userName} were not found" });
        }
        _repositoryFactory.UserRepository.Remove(users);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Deletes a user by employee id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/v4/users/{employeeId}
    ///
    /// Sample response body:
    ///     
    ///     Code 204 No Content
    /// 
    /// </remarks>
    /// <param name="employeeId">Employee Id</param>
    [SwaggerResponse(StatusCodes.Status204NoContent, Description = "User was successfully deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No user was found")]
    [HttpDelete("{employeeId:int}", Name = nameof(DeleteUserByEmployeeIdAsync))]
    public async Task<ActionResult> DeleteUserByEmployeeIdAsync([Required] int employeeId)
    {
        Logger.LogDebug(nameof(DeleteUserByEmployeeIdAsync));
        var user = await _repositoryFactory.UserRepository.GetUserAsync(employeeId).ConfigureAwait(false);
        if (user == null)
        {
            return NotFound(new { message = $"The user with EmployeeId {employeeId} was not found" });
        }
        _repositoryFactory.UserRepository.Remove(user);
        await _repositoryFactory.SaveAsync().ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Authenticate user
    /// </summary>
    /// <remarks>
    /// Public route that accepts HTTP POST requests containing the username and password in the body.
    /// If the username and password are correct then a JWT authentication token and the user details are returned.
    ///
    /// Sample request body:
    ///
    ///     {
    ///       "username": "johnw",
    ///       "password": "test"
    ///     }
    /// 
    /// Sample response body:
    ///
    ///     {
    ///       "employeeId": 1,
    ///       "token": "eyJhbGciOiJIUzI1NiIsInRb2hudyIs...blah blah blah...",
    ///       "username": "johnw",
    ///       "password": null,
    ///       "employeeFirstName": "Carl",
    ///       "employeeLastName": "Weiss"
    ///     }
    /// </remarks>
    /// <param name="model">AuthenticateModel model</param>
    [AllowAnonymous]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserAuthenticateDto), Description = "User with token")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Username or password is incorrect")]
    [HttpPost("authenticate", Name = nameof(AuthenticateUserAsync))]
    public async Task<IActionResult> AuthenticateUserAsync([FromBody] AuthenticateModel model)
    {
        var user = await _userService.AuthenticateAsync(model.Username, model.Password).ConfigureAwait(false);
        if (user == null)
        {
            return Unauthorized(new { message = "Username or password is incorrect" });
        }
        //return Ok(user);
        return Ok(user);
    }

    /// <summary>
    /// Get convenient bearer token for Swagger use
    /// </summary>
    /// <remarks>
    /// Public route that accepts HTTP POST requests containing the username and password in the body.
    /// If the username and password are correct then a authenticated JWT bearer token is returned.
    ///
    /// Sample request body:
    ///
    ///     {
    ///       "username": "johnw",
    ///       "password": "test"
    ///     }
    /// 
    /// Sample response body:
    ///
    ///     Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhbGFuZiIsImp0aSI6ImNlOWNmMGQ3L...
    ///     
    /// </remarks>
    /// <param name="model">AuthenticateModel model</param>
    [AllowAnonymous]
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Bearer token")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Username or password is incorrect")]
    [HttpPost("authToken", Name = nameof(AuthTokenAsync))]
    public async Task<IActionResult> AuthTokenAsync([FromBody] AuthenticateModel model)
    {
        var user = await _userService.AuthenticateAsync(model.Username, model.Password).ConfigureAwait(false);
        if (user == null)
        {
            return Unauthorized(new { message = "Username or password is incorrect" });
        }
        //return Ok(user);
        return Content("Bearer " + user.Token, MediaTypeNames.Text.Plain);
    }
}