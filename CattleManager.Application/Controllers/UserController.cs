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

    public UserController(IUserService userService)
    {
        _userService = userService;
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