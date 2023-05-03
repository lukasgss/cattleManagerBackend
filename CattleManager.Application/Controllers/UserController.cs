using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Application.Validation;
using CattleManager.Application.Application.Validation.User;
using Microsoft.AspNetCore.Mvc;

namespace CattleManager.Application.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserAuthorizationService _userAuthorizationService;

    public UserController(IUserService userService, IUserAuthorizationService userAuthorizationService)
    {
        _userService = userService;
        _userAuthorizationService = userAuthorizationService;
    }

    [Route("data/{id:guid}")]
    [HttpGet]
    public async Task<ActionResult<UserResponse>> GetUserDataById(Guid id)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        UserDataResponse userData = await _userService.GetUserDataByIdAsync(id, userId);
        return Ok(userData);
    }

    [HttpGet("dropdown")]
    public async Task<ActionResult<DropdownData>> GetUserForDropdownByNameOrLastName(string name)
    {
        IEnumerable<DropdownData> users = await _userService.GetUserByNameOrLastName(name);
        return Ok(users);
    }

    [Route("register")]
    [HttpPost]
    public async Task<ActionResult<UserResponse>> RegisterUser(RegisterUserRequest userRequest)
    {
        RegisterUserValidator requestValidator = new();
        var validationResult = requestValidator.Validate(userRequest);
        if (!validationResult.IsValid)
        {
            var modelStateDictionary = ValidationErrors.GenerateModelStateDictionary(validationResult);
            return ValidationProblem(modelStateDictionary);
        }

        return await _userService.RegisterUserAsync(userRequest);
    }

    [Route("login")]
    [HttpPost]
    public async Task<ActionResult<UserResponse>> LoginUser(LoginUserRequest userRequest)
    {
        LoginUserValidator requestValidator = new();
        var validationResult = requestValidator.Validate(userRequest);
        if (!validationResult.IsValid)
        {
            var modelStateDictionary = ValidationErrors.GenerateModelStateDictionary(validationResult);
            return ValidationProblem(modelStateDictionary);
        }
        return await _userService.LoginUserAsync(userRequest);
    }
}