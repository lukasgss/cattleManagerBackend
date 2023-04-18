using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Messages;

public interface IMessageService
{
    Task<PaginatedMessageResponse> GetAllMessagesToUserAsync(Guid senderId, Guid receiverId, int page);
    Task<MessageResponse> SendMessageAsync(MessageRequest messageRequest, Guid senderId);
    Task<MessageResponse> GetMessageByIdAsync(Guid messageId, Guid userId);
    Task<MessageNotificationAmount> GetAmountOfMessageNotificationsFromDistinctUsers(Guid userId);
}