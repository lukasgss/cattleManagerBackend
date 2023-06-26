using CattleManager.Application.Application.Common.Interfaces.CattleParents;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;

public record ConceptionResponse(Guid Id, DateOnly Date, CattleParentsResponse Father, CattleParentsResponse Mother);