using System.Security.Claims;

namespace CattleManager.Application.Application.Common.Interfaces.Authorization;

public interface IUserAuthorizationService
{
    string GetUserIdFromJwtToken(ClaimsPrincipal user);
}