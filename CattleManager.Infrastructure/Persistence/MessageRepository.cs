using CattleManager.Application.Application.Common.Constants;
using CattleManager.Application.Application.Common.Interfaces.Entities.Messages;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    private readonly AppDbContext _dbContext;
    public MessageRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public double GetAmountOfPages(Guid senderId, Guid receiverId)
    {
        return Math.Ceiling(_dbContext.Messages
            .Where(message => message.Receiver.Id == receiverId
                && message.Sender.Id == senderId)
            .Count() / (double)GlobalConstants.ResultsPerPage);
    }

    public async Task<IEnumerable<Message>> GetAllMessagesToUser(Guid senderId, Guid receiverId, int page)
    {
        return await _dbContext.Messages
            .AsNoTracking()
            .Where(message => message.Receiver.Id == receiverId
                && message.Sender.Id == senderId)
            .OrderByDescending(message => message.Date)
            .Skip((page - 1) * GlobalConstants.ResultsPerPage)
            .Take(GlobalConstants.ResultsPerPage)
            .ToListAsync();
    }

    public async Task<Message?> GetMessageByIdAsync(Guid messageId, Guid userId)
    {
        return await _dbContext.Messages
            .AsNoTracking()
            .FirstOrDefaultAsync(message => message.Id == messageId
                && (message.Receiver.Id == userId || message.Sender.Id == userId));
    }

    public async Task<int> GetAmountOfMessageNotifications(Guid userId)
    {
        return await _dbContext.Messages
            .AsNoTracking()
            .Where(message => message.Receiver.Id == userId
                && !message.HasBeenRead)
            .Select(message => new { message.SenderId })
            .Distinct()
            .CountAsync();
    }

    public async Task MarkMessagesAsRead(Guid userId, Guid senderId)
    {
        await _dbContext.Messages
            .Where(message => message.SenderId == senderId
                && message.ReceiverId == userId)
            .ExecuteUpdateAsync(message => message.SetProperty(m => m.HasBeenRead, true));
        await CommitAsync();
    }
}