using System;
using System.Threading.Tasks;
using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Authentication;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Services.Entities;
using CattleManager.Application.Domain.Entities;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests;

public class UserServiceTests
{
    private readonly IUserService _sut;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IPasswordService _passwordServiceMock;
    private readonly IJwtTokenGenerator _jwtTokenGeneratorMock;
    private readonly RegisterUserRequest _validUserRequest = new(
        FirstName: "firstName",
        LastName: "lastName",
        Username: "username",
        Email: "email@email.com",
        Password: "password",
        ConfirmPassword: "password");
    private readonly LoginUserRequest _validLoginRequest = new("username", "password");
    private readonly User _userMock = new()
    {
        Id = Guid.NewGuid(),
        FirstName = "firstName",
        LastName = "lastName",
        Email = "email@email.com",
        Username = "username",
        Password = "password"
    };
    public UserServiceTests()
    {
        _userRepositoryMock = A.Fake<IUserRepository>();
        _passwordServiceMock = A.Fake<IPasswordService>();
        _jwtTokenGeneratorMock = A.Fake<IJwtTokenGenerator>();
        _sut = new UserService(_userRepositoryMock, _passwordServiceMock, _jwtTokenGeneratorMock);
    }

    [Fact]
    public async Task Does_Not_Register_User_When_Password_And_Confirm_Password_Dont_Match()
    {
        var userToRegister = new RegisterUserRequest(
            FirstName: "firstName",
            LastName: "lastName",
            Username: "username",
            Email: "email",
            Password: "password",
            ConfirmPassword: "differentThanPassword"
        );

        async Task result() => await _sut.RegisterUserAsync(userToRegister);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Campos de senha e confirmar senha n??o coincidem.", exception.Message);
    }

    [Fact]
    public async Task Does_Not_Register_User_When_Username_Already_Exists()
    {
        A.CallTo(() => _userRepositoryMock.GetUserByUsernameAsync("username")).Returns(_userMock);

        async Task result() => await _sut.RegisterUserAsync(_validUserRequest);
        var exception = await Assert.ThrowsAsync<ConflictException>(result);
        Assert.Equal("Usu??rio com esse nome de usu??rio j?? existe.", exception.Message);
    }

    [Fact]
    public async Task Does_Not_Register_User_When_Email_Already_Exists()
    {
        User? nullUser = null;

        A.CallTo(() => _userRepositoryMock.GetUserByUsernameAsync(_validUserRequest.Username)).Returns(nullUser);
        A.CallTo(() => _userRepositoryMock.GetUserByEmailAsync(_validUserRequest.Email)).Returns(_userMock);

        async Task result() => await _sut.RegisterUserAsync(_validUserRequest);
        var exception = await Assert.ThrowsAsync<ConflictException>(result);
        Assert.Equal("Usu??rio com esse endere??o de e-mail j?? existe.", exception.Message);
    }

    [Fact]
    public async Task Registers_User_When_Data_Is_Valid()
    {
        User? nullUser = null;
        A.CallTo(() => _userRepositoryMock.GetUserByUsernameAsync(_validUserRequest.Username)).Returns(nullUser);
        A.CallTo(() => _userRepositoryMock.GetUserByEmailAsync(_validUserRequest.Email)).Returns(nullUser);
        Guid userId = Guid.NewGuid();
        A.CallTo(() => _jwtTokenGeneratorMock.GenerateToken(userId, _validUserRequest.FirstName, _validUserRequest.LastName)).Returns("jwtToken");
        var user = await _sut.RegisterUserAsync(_validUserRequest);

        Assert.NotNull(user);
        Assert.NotNull(user.Id.ToString());
        Assert.NotEqual(Guid.Empty.ToString(), user.Id.ToString());
        Assert.Equal(_validUserRequest.FirstName, user.FirstName);
        Assert.Equal(_validUserRequest.LastName, user.LastName);
        Assert.Equal(_validUserRequest.Email, user.Email);
        Assert.Equal(_validUserRequest.Username, user.Username);
        Assert.NotNull(user.Token);
    }

    [Fact]
    public async Task Does_Not_Login_With_Incorrect_Username()
    {
        User? nullUser = null;
        A.CallTo(() => _userRepositoryMock.GetUserByUsernameAsync("wrongUsername")).Returns(nullUser);

        async Task result() => await _sut.LoginUserAsync(_validLoginRequest);

        var exception = await Assert.ThrowsAsync<UnauthorizedException>(result);
        Assert.Equal("Credenciais inv??lidas.", exception.Message);
    }

    [Fact]
    public async Task Does_Not_Login_With_Incorrect_Password()
    {
        A.CallTo(() => _userRepositoryMock.GetUserByUsernameAsync(_validLoginRequest.Username)).Returns(_userMock);
        A.CallTo(() => _passwordServiceMock.ComparePassword(_validLoginRequest.Password, "hashedPassword")).Returns(false);

        async Task result() => await _sut.LoginUserAsync(_validLoginRequest);

        var exception = await Assert.ThrowsAsync<UnauthorizedException>(result);
        Assert.Equal("Credenciais inv??lidas.", exception.Message);
    }

    [Fact]
    public async Task Logs_in_With_Correct_Credentials()
    {
        A.CallTo(() => _userRepositoryMock.GetUserByUsernameAsync(_validLoginRequest.Username)).Returns(_userMock);
        A.CallTo(() => _passwordServiceMock.ComparePassword(_validLoginRequest.Password, _validLoginRequest.Password)).Returns(true);
        A.CallTo(() => _jwtTokenGeneratorMock.GenerateToken(_userMock.Id, _userMock.FirstName, _userMock.LastName)).Returns("jwtToken");

        var userResponse = await _sut.LoginUserAsync(_validLoginRequest);

        Assert.NotNull(userResponse);
        Assert.Equal(_userMock.Id, userResponse.Id);
        Assert.Equal(_userMock.FirstName, userResponse.FirstName);
        Assert.Equal(_userMock.LastName, userResponse.LastName);
        Assert.Equal(_userMock.Email, userResponse.Email);
        Assert.Equal(_userMock.Username, userResponse.Username);
        Assert.Equal("jwtToken", userResponse.Token);
    }
}