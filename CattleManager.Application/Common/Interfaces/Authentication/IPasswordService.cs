namespace CattleManager.Application.Application.Common.Interfaces.Authentication;

public interface IPasswordService
{
    bool ComparePassword(string plainTextPassword, string hashedPassword);
    string HashPassword(string plainTextPassword);
}