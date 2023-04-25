namespace CattleManager.Application.Application.Common.Interfaces.ServiceValidations;

public interface IServiceValidations
{
    void ValidateMonth(int month);
    void ValidateDate(int month, int year);
}