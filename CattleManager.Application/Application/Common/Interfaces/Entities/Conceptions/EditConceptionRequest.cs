namespace CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;

public record EditConceptionRequest(Guid Id, DateOnly Date, Guid FatherId, Guid MotherId);