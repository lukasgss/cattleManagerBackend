using System.Web;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Authentication;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Application.Helpers;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Services.Entities;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IGuidProvider _guidProvider;

    public UserService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IJwtTokenGenerator jwtTokenGenerator,
        IGuidProvider guidProvider)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _guidProvider = guidProvider;
    }

    public async Task<IEnumerable<DropdownData>> GetUserByNameOrLastName(string fullName)
    {
        if (fullName.Length == 0)
            throw new BadRequestException("Especifique o nome a ser buscado.");

        string nameWithoutDiacritics = StringExtensions.RemoveDiacritics(fullName);
        string nameWithDatabaseWildcards = StringExtensions.AddDatabaseWildcards(nameWithoutDiacritics);
        string[] firstNameAndLastNameSplit = nameWithDatabaseWildcards.Split(" ");
        string firstName = firstNameAndLastNameSplit[0];
        string lastName = string.Concat(firstNameAndLastNameSplit.Skip(1).ToArray());

        bool matchBoth = firstName.Length > 0 && lastName.Length > 0;

        return await _userRepository.GetUserByNameOrLastNameForDropdown(firstName, lastName, matchBoth);
    }

    public async Task<UserDataResponse> GetUserDataByIdAsync(Guid userIdToGet, Guid userId)
    {
        if (userId != userIdToGet)
            throw new NotFoundException("Usuário com o id especificado não existe.");

        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            throw new NotFoundException("Usuário com o id especificado não existe.");

        return new UserDataResponse(
            Id: user.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Email: user.Email
        );
    }

    public async Task<UserResponse> LoginUserAsync(LoginUserRequest userRequest)
    {
        var user = await _userRepository.GetUserByEmailAsync(userRequest.Email);
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
            jwtToken);
    }

    public async Task<UserResponse> RegisterUserAsync(RegisterUserRequest userRequest)
    {
        if (userRequest.Password != userRequest.ConfirmPassword)
            throw new BadRequestException("Campos de senha e confirmar senha não coincidem.");

        var userToRegisterByEmail = await _userRepository.GetUserByEmailAsync(userRequest.Email);
        if (userToRegisterByEmail is not null)
            throw new ConflictException("Usuário com esse endereço de e-mail já existe.");

        string hashedPassword = _passwordService.HashPassword(userRequest.Password);
        User user = new()
        {
            Id = _guidProvider.NewGuid(),
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
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
            jwtToken);
    }
}