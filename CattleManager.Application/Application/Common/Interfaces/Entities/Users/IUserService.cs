using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Users;

public interface IUserService
{
    Task<UserDataResponse> GetUserDataByIdAsync(Guid userIdToGet, Guid userId);
    Task<UserResponse> RegisterUserAsync(RegisterUserRequest userRequest);
    Task<UserResponse> LoginUserAsync(LoginUserRequest userRequest);
    Task<IEnumerable<DropdownData>> GetUserByNameOrLastName(string name);
}