using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Messages;

public interface IMessageRepository : IGenericRepository<Message>
{
    double GetAmountOfPages(Guid senderId, Guid receiverId);
    Task<IEnumerable<Message>> GetAllMessagesToUser(Guid senderId, Guid receiverId, int page);
    Task<Message?> GetMessageByIdAsync(Guid messageId, Guid userId);
    Task<int> GetAmountOfMessageNotifications(Guid userId);
    Task MarkMessagesAsRead(Guid userId, Guid senderId);
}