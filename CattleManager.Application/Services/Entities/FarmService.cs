using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Common.Interfaces.Entities.Farms;
using CattleManager.Application.Domain.Entities;
using CattleManager.Domain.Entities;

namespace CattleManager.Application.Services.Entities;

public class FarmService : IFarmService
{
    private readonly IFarmRepository _farmRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGuidProvider _guidProvider;
    private readonly IMapper _mapper;

    public FarmService(
        IFarmRepository farmRepository,
        IUserRepository userRepository,
        IGuidProvider guidProvider,
        IMapper mapper)
    {
        _farmRepository = farmRepository;
        _userRepository = userRepository;
        _guidProvider = guidProvider;
        _mapper = mapper;
    }

    public async Task<FarmResponse> GetFarmByIdAsync(Guid userId, Guid farmId)
    {
        Farm? farm = await _farmRepository.GetFarmByIdAsync(userId, farmId);
        if (farm is null)
            throw new NotFoundException("Fazenda com o id especificado não foi encontrada.");

        return _mapper.Map<FarmResponse>(farm);
    }

    public async Task<FarmResponse> CreateFarmAsync(Guid userId, CreateFarmRequest createFarmRequest)
    {
        User? userCreatingFarm = await _userRepository.GetByIdAsync(userId);
        if (userCreatingFarm is null)
            throw new NotFoundException("Usuário para criar a fazenda não foi encontrado.");

        List<FarmOwner> defaultFarmOwners = GenerateDefaultFarmOwner(userId);
        Farm farmToCreate = new()
        {
            Id = _guidProvider.NewGuid(),
            Name = createFarmRequest.Name,
            Owners = defaultFarmOwners
        };
        _farmRepository.Add(farmToCreate);
        await _farmRepository.CommitAsync();

        return new FarmResponse()
        {
            Id = farmToCreate.Id,
            Name = farmToCreate.Name
        };
    }

    public async Task<FarmResponse> EditFarmAsync(Guid userId, EditFarmRequest editFarmRequest, Guid routeId)
    {
        if (editFarmRequest.Id != routeId)
            throw new BadRequestException("Id da rota não coincide com o id da fazenda especificada.");

        Farm? farmToEdit = await _farmRepository.GetFarmByIdAsync(userId, editFarmRequest.Id, false);
        if (farmToEdit is null)
            throw new NotFoundException("Fazenda com o id especificado não foi encontrada.");

        Farm editedFarm = new()
        {
            Id = editFarmRequest.Id,
            Name = editFarmRequest.Name,
            Members = farmToEdit.Members,
            Owners = farmToEdit.Owners
        };

        _farmRepository.Update(editedFarm);
        await _farmRepository.CommitAsync();

        return new FarmResponse()
        {
            Id = editedFarm.Id,
            Name = editedFarm.Name
        };
    }

    public async Task DeleteFarmAsync(Guid farmId, Guid userId)
    {
        Farm? farmToDelete = await _farmRepository.GetFarmByIdAsync(userId, farmId);
        if (farmToDelete is null)
            throw new NotFoundException("Fazenda com o id especificado não foi encontrada.");

        _farmRepository.Delete(farmToDelete);
        await _farmRepository.CommitAsync();
    }

    private List<FarmOwner> GenerateDefaultFarmOwner(Guid userId)
    {
        Guid farmId = _guidProvider.NewGuid();
        return new List<FarmOwner>()
        {
            new FarmOwner()
            {
                FarmId = farmId,
                OwnerId = userId
            }
        };
    }
}