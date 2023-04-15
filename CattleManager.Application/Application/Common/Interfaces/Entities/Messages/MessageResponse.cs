namespace CattleManager.Application.Application.Common.Interfaces.Entities.Messages;

public record MessageResponse(
    Guid Id,
    string Content,
    DateTime Date,
    bool HasBeenRead,
    Guid SenderId,
    Guid ReceiverId);