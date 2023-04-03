namespace CattleManager.Application.Application.Common.Interfaces.Entities.Users;

public record UserDataResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);