namespace CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;

public record CreateConceptionRequest(DateOnly Date, Guid FatherId, Guid MotherId);