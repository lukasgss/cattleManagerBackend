namespace CattleManager.Application.Application.Common.Interfaces.Entities.Users;

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Username,
    string Token);