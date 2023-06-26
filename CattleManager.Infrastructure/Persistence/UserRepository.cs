using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Email == email);
    }

    public async Task<IEnumerable<DropdownData>> GetUserByNameOrLastNameForDropdown(string firstName, string lastName, bool matchBoth)
    {
        return matchBoth ? await _dbContext.Users
            .AsNoTracking()
            .Where(user => EF.Functions.ILike(EF.Functions.Unaccent(user.FirstName), $"{firstName}")
                && EF.Functions.ILike(EF.Functions.Unaccent(user.LastName), $"{lastName}"))
            .Select(user => new DropdownData() { Text = $"{user.FirstName} {user.LastName}", Value = user.Id })
            .ToListAsync()
            :
            await _dbContext.Users
            .AsNoTracking()
            .Where(user => EF.Functions.ILike(EF.Functions.Unaccent(user.FirstName), $"{firstName}")
                || EF.Functions.ILike(EF.Functions.Unaccent(user.LastName), $"{lastName}")
                || EF.Functions.ILike(EF.Functions.Unaccent(user.LastName), $"{firstName}")
                || EF.Functions.ILike(EF.Functions.Unaccent(user.FirstName), $"{lastName}"))
            .Select(user => new DropdownData() { Text = $"{user.FirstName} {user.LastName}", Value = user.Id })
            .ToListAsync();
    }
}