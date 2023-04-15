namespace CattleManager.Application.Application.Common.Interfaces.Entities.Messages;

public record MessageRequest(
    string Content,
    Guid SenderId,
    Guid ReceiverId);