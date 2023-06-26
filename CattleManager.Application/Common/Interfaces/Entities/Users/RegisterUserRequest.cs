namespace CattleManager.Application.Application.Common.Interfaces.Entities.Users;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword);