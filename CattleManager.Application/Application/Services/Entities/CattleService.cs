using CattleManager.Application.Application.Common.Enums;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.CattleBreeds;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.Owners;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Application.Services.General;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Services.Entities;

public class CattleService : ICattleService
{
    private readonly ICattleRepository _cattleRepository;
    private readonly IGuidProvider _guidProvider;

    public CattleService(ICattleRepository cattleRepository, IGuidProvider guidProvider)
    {
        _cattleRepository = cattleRepository;
        _guidProvider = guidProvider;
    }

    public async Task<IEnumerable<CattleResponse>> GetAllCattlesFromOwner(Guid ownerId)
    {
        IEnumerable<Cattle> cattlesFromOwner = await _cattleRepository.GetAllCattlesFromOwner(ownerId);
        List<CattleResponse> cattlesFromOwnerResponse = new();
        foreach (var ownedCattle in cattlesFromOwner)
        {
            var cattleResponse = GenerateCattleResponseDto(ownedCattle);
            cattlesFromOwnerResponse.Add(cattleResponse);
        }

        return cattlesFromOwnerResponse;
    }

    public async Task<CattleResponse> GetCattleById(Guid cattleId, Guid userId)
    {
        var cattle = await _cattleRepository.GetCattleById(cattleId, userId);
        if (cattle is null)
            throw new NotFoundException("Animal com o ID especificado não foi encontrado.");

        return GenerateCattleResponseDto(cattle);
    }

    public async Task<IEnumerable<CattleResponse>> GetCattleByNameAsync(string cattleName, Guid userId)
    {
        var cattleByName = await _cattleRepository.GetCattleByName(cattleName, userId);

        List<CattleResponse> cattleResponse = new();
        foreach (Cattle cattle in cattleByName)
        {
            cattleResponse.Add(GenerateCattleResponseDto(cattle));
        }

        return cattleResponse;
    }

    public async Task<CattleResponse> CreateCattle(CattleRequest cattleRequest, Guid userId)
    {
        var cattleByName = await _cattleRepository.GetCattleByName(cattleRequest.Name, userId);
        if (cattleByName.Any())
            throw new ConflictException("Gado com esse nome já existe.");

        if (cattleRequest.FatherId is not null)
        {
            var cattleFather = await _cattleRepository.GetByIdAsync(cattleRequest.FatherId.Value);
            if (cattleFather is null)
                throw new NotFoundException("Pai especificado não foi encontrado.");
            if (cattleFather.SexId == (byte)Gender.Female)
                throw new BadRequestException("Pai do animal não pode ser do sexo feminino.");
        }
        if (cattleRequest.MotherId is not null)
        {
            var cattleMother = await _cattleRepository.GetByIdAsync(cattleRequest.MotherId.Value);
            if (cattleMother is null)
                throw new NotFoundException("Mãe especificada não foi encontrada.");
            if (cattleMother.SexId == (byte)Gender.Male)
                throw new BadRequestException("Mãe do animal não pode ser do sexo masculino.");
        }

        Cattle cattleToRegister = GenerateCattleToBeRegistered(cattleRequest);

        GenerateListOfCattleBreedRequest(cattleRequest, cattleToRegister);
        GenerateListOfOwnerRequest(cattleRequest, cattleToRegister);

        _cattleRepository.Add(cattleToRegister);
        await _cattleRepository.CommitAsync();

        var createdCattle = await GetCattleById(cattleToRegister.Id, userId);
        if (createdCattle is null)
            throw new Exception("Não foi possível retornar os dados do gado.");

        return createdCattle;
    }

    private Cattle GenerateCattleToBeRegistered(CattleRequest cattleRequest)
    {
        Guid cattleId = _guidProvider.NewGuid();
        return new()
        {
            Id = cattleId,
            Name = cattleRequest.Name,
            PurchaseDate = cattleRequest.PurchaseDate,
            ConceptionDate = cattleRequest.ConceptionDate,
            DateOfBirth = cattleRequest.DateOfBirth,
            YearOfBirth = cattleRequest.YearOfBirth,
            Image = cattleRequest.Image,
            DateOfDeath = cattleRequest.DateOfDeath,
            CauseOfDeath = cattleRequest.CauseOfDeath,
            DateOfSale = cattleRequest.DateOfSale,
            PriceInCentsInReais = cattleRequest.PriceInCentsInReais,
            CattleOwners = new List<CattleOwner>(),
            FatherId = cattleRequest.FatherId,
            MotherId = cattleRequest.MotherId,
            SexId = cattleRequest.SexId,
            CattleBreeds = new List<CattleBreed>()
        };
    }

    private static void GenerateListOfOwnerRequest(CattleRequest cattleRequest, Cattle cattleToRegister)
    {
        foreach (Guid ownerId in cattleRequest.OwnersIds)
        {
            if (cattleToRegister.CattleOwners.Any(owner => owner.Id == ownerId))
                throw new BadRequestException("Um usuário não pode ser mencionado duas vezes como dono.");

            CattleOwner owner = new()
            {
                UserId = ownerId,
                CattleId = cattleToRegister.Id
            };
            cattleToRegister.CattleOwners.Add(owner);
        }
    }

    private static void GenerateListOfCattleBreedRequest(CattleRequest cattleRequest, Cattle cattleToRegister)
    {
        decimal sumOfBreeds = 0;

        foreach (CattleBreedRequest breedRequest in cattleRequest.Breeds)
        {
            sumOfBreeds += breedRequest.QuantityInPercentage;
            CattleBreed breed = new()
            {
                QuantityInPercentage = breedRequest.QuantityInPercentage,
                CattleId = cattleToRegister.Id,
                BreedId = breedRequest.BreedId
            };
            cattleToRegister.CattleBreeds.Add(breed);
        }

        if (sumOfBreeds > 1)
            throw new BadRequestException("Soma dos valores das raças é maior do que 100%.");
    }

    private static CattleResponse GenerateCattleResponseDto(Cattle cattle)
    {
        return new CattleResponse(
            cattle.Id,
            cattle.Name,
            cattle.FatherId,
            cattle.Father?.Name,
            cattle.MotherId,
            cattle.Mother?.Name,
            cattle.Sex.Gender,
            cattle.CattleBreeds.
            Select(x => new CattleBreedResponse(
                x.Breed.Name, DecimalToFractionService.RealToFraction((double)x.QuantityInPercentage))),
            cattle.PurchaseDate,
            cattle.ConceptionDate,
            cattle.DateOfBirth,
            cattle.YearOfBirth,
            cattle.Image,
            cattle.DateOfDeath,
            cattle.CauseOfDeath,
            cattle.DateOfSale,
            cattle.CattleOwners.Select(x => new CattleOwnerResponse(x.User.FirstName, x.User.LastName))
        );
    }
}