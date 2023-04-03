using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
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
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        UserDataResponse userData = await _userService.GetUserDataByIdAsync(id, new Guid(userId));
        return Ok(userData);
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