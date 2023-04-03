using System;
using System.Threading.Tasks;
using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Authentication;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
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
    private readonly IGuidProvider _guidProviderMock;
    private readonly RegisterUserRequest _validUserRequest = new(
        FirstName: "firstName",
        LastName: "lastName",
        Email: "email@email.com",
        Password: "password",
        ConfirmPassword: "password");
    private readonly LoginUserRequest _validLoginRequest = new("email@email.com", "password");
    private readonly User _userMock = new()
    {
        Id = Guid.NewGuid(),
        FirstName = "firstName",
        LastName = "lastName",
        Email = "email@email.com",
        Password = "password"
    };
    public UserServiceTests()
    {
        _userRepositoryMock = A.Fake<IUserRepository>();
        _passwordServiceMock = A.Fake<IPasswordService>();
        _jwtTokenGeneratorMock = A.Fake<IJwtTokenGenerator>();
        _guidProviderMock = A.Fake<IGuidProvider>();
        _sut = new UserService(_userRepositoryMock, _passwordServiceMock, _jwtTokenGeneratorMock, _guidProviderMock);
    }

    [Fact]
    public async Task Does_Not_Register_User_When_Password_And_Confirm_Password_Dont_Match()
    {
        var userToRegister = new RegisterUserRequest(
            FirstName: "firstName",
            LastName: "lastName",
            Email: "email",
            Password: "password",
            ConfirmPassword: "differentThanPassword"
        );

        async Task result() => await _sut.RegisterUserAsync(userToRegister);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Campos de senha e confirmar senha não coincidem.", exception.Message);
    }

    [Fact]
    public async Task Register_User_With_Existent_Email_Throws_ConflictException()
    {
        A.CallTo(() => _userRepositoryMock.GetUserByEmailAsync("email@email.com")).Returns(_userMock);

        async Task result() => await _sut.RegisterUserAsync(_validUserRequest);

        var exception = await Assert.ThrowsAsync<ConflictException>(result);
        Assert.Equal("Usuário com esse endereço de e-mail já existe.", exception.Message);
    }

    [Fact]
    public async Task Registers_User_When_Data_Is_Valid()
    {
        User? nullUser = null;
        A.CallTo(() => _userRepositoryMock.GetUserByEmailAsync(_validUserRequest.Email)).Returns(nullUser);
        Guid userId = Guid.NewGuid();
        A.CallTo(() => _jwtTokenGeneratorMock.GenerateToken(userId, _validUserRequest.FirstName, _validUserRequest.LastName)).Returns("jwtToken");
        A.CallTo(() => _guidProviderMock.NewGuid()).Returns(userId);
        UserResponse expectedUserResponse = GenerateUserResponseFromUserRequest(userId, _validUserRequest);

        var userResponse = await _sut.RegisterUserAsync(_validUserRequest);

        Assert.Equivalent(expectedUserResponse, userResponse);
    }

    [Fact]
    public async Task Does_Not_Login_With_Incorrect_Email()
    {
        User? nullUser = null;
        A.CallTo(() => _userRepositoryMock.GetUserByEmailAsync("wrongEmail@email.com")).Returns(nullUser);

        async Task result() => await _sut.LoginUserAsync(_validLoginRequest);

        var exception = await Assert.ThrowsAsync<UnauthorizedException>(result);
        Assert.Equal("Credenciais inválidas.", exception.Message);
    }

    [Fact]
    public async Task Does_Not_Login_With_Incorrect_Password()
    {
        A.CallTo(() => _userRepositoryMock.GetUserByEmailAsync(_validLoginRequest.Email)).Returns(_userMock);
        A.CallTo(() => _passwordServiceMock.ComparePassword(_validLoginRequest.Password, "hashedPassword")).Returns(false);

        async Task result() => await _sut.LoginUserAsync(_validLoginRequest);

        var exception = await Assert.ThrowsAsync<UnauthorizedException>(result);
        Assert.Equal("Credenciais inválidas.", exception.Message);
    }

    [Fact]
    public async Task Logs_in_With_Correct_Credentials()
    {
        A.CallTo(() => _userRepositoryMock.GetUserByEmailAsync(_validLoginRequest.Email)).Returns(_userMock);
        A.CallTo(() => _passwordServiceMock.ComparePassword(_validLoginRequest.Password, _validLoginRequest.Password)).Returns(true);
        A.CallTo(() => _jwtTokenGeneratorMock.GenerateToken(_userMock.Id, _userMock.FirstName, _userMock.LastName)).Returns("jwtToken");
        UserResponse expectedUserResponse = GenerateUserResponseFromUser(_userMock);

        var userResponse = await _sut.LoginUserAsync(_validLoginRequest);

        Assert.Equivalent(expectedUserResponse, userResponse);
    }

    private static UserResponse GenerateUserResponseFromUserRequest(Guid userId, RegisterUserRequest userRequest)
    {
        return new UserResponse(
            Id: userId,
            FirstName: userRequest.FirstName,
            LastName: userRequest.LastName,
            Email: userRequest.Email,
            Token: "jwtToken");
    }

    private static UserResponse GenerateUserResponseFromUser(User user)
    {
        return new UserResponse(
            Id: user.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Email: user.Email,
            Token: "jwtToken");
    }
}