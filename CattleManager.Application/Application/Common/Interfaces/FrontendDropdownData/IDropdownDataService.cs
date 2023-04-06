namespace CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;

public interface IDropdownDataService
{
    Task<IEnumerable<DropdownDataResponse>> GetMaleCattleByName(string name, Guid userId);
    Task<IEnumerable<DropdownDataResponse>> GetFemaleCattleByName(string name, Guid userId);
}