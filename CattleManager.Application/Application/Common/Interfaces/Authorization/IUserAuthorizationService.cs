using System.Security.Claims;

namespace CattleManager.Application.Application.Common.Interfaces.Authorization;

public interface IUserAuthorizationService
{
    Guid GetUserIdFromJwtToken(ClaimsPrincipal user);
}