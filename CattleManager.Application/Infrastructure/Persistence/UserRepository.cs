using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
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

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Username == username);
    }
}