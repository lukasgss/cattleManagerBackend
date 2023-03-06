using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Authentication;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Services.Entities;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<UserResponse> LoginUserAsync(LoginUserRequest userRequest)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userRequest.Username);
        if (user is null)
            throw new UnauthorizedException("Credenciais inválidas.");

        bool passwordsMatch = _passwordService.ComparePassword(userRequest.Password, user.Password);
        if (!passwordsMatch)
            throw new UnauthorizedException("Credenciais inválidas.");

        var jwtToken = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);

        return new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Username,
            jwtToken);
    }

    public async Task<UserResponse> RegisterUserAsync(RegisterUserRequest userRequest)
    {
        if (userRequest.Password != userRequest.ConfirmPassword)
            throw new BadRequestException("Campos de senha e confirmar senha não coincidem.");

        var userToRegisterByUsername = await _userRepository.GetUserByUsernameAsync(userRequest.Username);
        if (userToRegisterByUsername is not null)
            throw new ConflictException("Usuário com esse nome de usuário já existe.");

        var userToRegisterByEmail = await _userRepository.GetUserByEmailAsync(userRequest.Email);
        if (userToRegisterByEmail is not null)
            throw new ConflictException("Usuário com esse endereço de e-mail já existe.");

        string hashedPassword = _passwordService.HashPassword(userRequest.Password);
        User user = new()
        {
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
            Username = userRequest.Username,
            Email = userRequest.Email,
            Password = hashedPassword
        };
        _userRepository.Add(user);
        await _userRepository.CommitAsync();

        var jwtToken = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);

        return new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Username,
            jwtToken);
    }
}