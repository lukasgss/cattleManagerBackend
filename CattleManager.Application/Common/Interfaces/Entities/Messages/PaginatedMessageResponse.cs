using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Messages;

public record PaginatedMessageResponse(IEnumerable<Message> Message, int CurrentPage, double Pages);