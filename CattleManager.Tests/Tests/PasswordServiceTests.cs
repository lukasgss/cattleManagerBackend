using CattleManager.Application.Application.Common.Interfaces.Authentication;
using CattleManager.Application.Application.Services.Authentication;
using Xunit;

namespace CattleManager.Tests;

public class PasswordServiceTests
{
    private readonly IPasswordService _sut;

    public PasswordServiceTests()
    {
        _sut = new PasswordService();
    }

    [Fact]
    public void Returns_False_If_Passwords_To_Compare_Are_Different()
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("differentPassword");
        bool comparePassword = _sut.ComparePassword("plainTextPassword", passwordHash);

        Assert.False(comparePassword);
    }

    [Fact]
    public void Returns_True_If_Passwords_To_Compare_Are_Equal()
    {
        const string password = "password";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        bool comparePassword = _sut.ComparePassword(password, passwordHash);

        Assert.True(comparePassword);
    }

    [Fact]
    public void Hashes_Password()
    {
        const int bcryptPasswordHashLength = 60;

        var password = _sut.HashPassword("plainTextPassword");

        Assert.NotNull(password);
        Assert.Equal(bcryptPasswordHashLength, password.Length);
    }
}