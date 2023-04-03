namespace CattleManager.Application.Application.Common.Interfaces.Entities.Users;

public interface IUserService
{
    Task<UserDataResponse> GetUserDataByIdAsync(Guid userIdToGet, Guid userId);
    Task<UserResponse> RegisterUserAsync(RegisterUserRequest userRequest);
    Task<UserResponse> LoginUserAsync(LoginUserRequest userRequest);
}