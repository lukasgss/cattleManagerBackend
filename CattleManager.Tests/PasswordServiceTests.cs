using System;
using CattleManager.Application.Application.Common.Interfaces.Authentication;
using CattleManager.Application.Application.Services.Authentication;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests;

public class PasswordServiceTests
{
    private readonly IPasswordService _sut;
    private readonly IPasswordService _passwordServiceMock;

    public PasswordServiceTests()
    {
        _sut = new PasswordService();
        _passwordServiceMock = A.Fake<IPasswordService>();
    }

    [Fact]
    public void ShouldReturnFalseIfPasswordsToCompareAreDifferent()
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("differentPassword");
        bool comparePassword = _sut.ComparePassword("plainTextPassword", passwordHash);

        Assert.False(comparePassword);
    }

    [Fact]
    public void ShouldReturnTrueIfPasswordsToCompareAreEqual()
    {
        const string password = "password";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        bool comparePassword = _sut.ComparePassword(password, passwordHash);

        Assert.True(comparePassword);
    }

    [Fact]
    public void ShouldHashPassword()
    {
        const int bcryptPasswordHashLength = 60;

        var password = _sut.HashPassword("plainTextPassword");

        Assert.NotNull(password);
        Assert.Equal(bcryptPasswordHashLength, password.Length);
    }
}