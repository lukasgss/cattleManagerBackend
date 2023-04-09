using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Users;

public interface IUserRepository : IGenericRepository<User>
{
    Task<IEnumerable<DropdownData>> GetUserByNameOrLastNameForDropdown(string firstName, string lastName, bool matchBoth);
    Task<User?> GetUserByEmailAsync(string email);
}