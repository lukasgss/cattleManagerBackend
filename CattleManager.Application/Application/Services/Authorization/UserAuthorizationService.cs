using System.Security.Claims;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Authorization;

namespace CattleManager.Application.Application.Services.Authorization;

public class UserAuthorizationService : IUserAuthorizationService
{
    public string GetUserIdFromJwtToken(ClaimsPrincipal user)
    {
        string? userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            throw new ForbiddenException("Você não possui permissão.");

        return userId;
    }
}