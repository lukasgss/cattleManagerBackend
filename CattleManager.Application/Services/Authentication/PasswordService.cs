using CattleManager.Application.Application.Common.Interfaces.Authentication;

namespace CattleManager.Application.Application.Services.Authentication;

public class PasswordService : IPasswordService
{
    public bool ComparePassword(string plainTextPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}