namespace CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;

public record ConceptionResponse(Guid Id, DateOnly Date, Guid FatherId, Guid MotherId);