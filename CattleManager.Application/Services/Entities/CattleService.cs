using CattleManager.Application.Application.Common.Enums;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Common;
using CattleManager.Application.Application.Common.Interfaces.Entities.CattleBreeds;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.Owners;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Application.Helpers;
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

    public async Task<PaginatedCattleResponse> GetAllCattleFromOwner(Guid ownerId, int page)
    {
        double amountOfPages = _cattleRepository.GetAmountOfPages(ownerId);
        if ((page > amountOfPages && amountOfPages > 0) || page < 1)
            throw new BadRequestException($"Resultado possui {amountOfPages} página(s), insira um valor entre 1 e o número de páginas.");

        IEnumerable<Cattle> cattleFromOwner = await _cattleRepository.GetAllCattleFromOwner(ownerId, page);
        List<CattleResponse> cattleFromOwnerResponse = new();
        foreach (var ownedCattle in cattleFromOwner)
        {
            var cattleResponse = GenerateCattleResponseDto(ownedCattle);
            cattleFromOwnerResponse.Add(cattleResponse);
        }

        return new PaginatedCattleResponse(cattleFromOwnerResponse, page, amountOfPages);
    }

    public async Task<CattleResponse> GetCattleById(Guid cattleId, Guid userId)
    {
        var cattle = await _cattleRepository.GetCattleById(cattleId, userId, true);
        if (cattle is null)
            throw new NotFoundException("Animal com o ID especificado não foi encontrado.");

        return GenerateCattleResponseDto(cattle);
    }

    public async Task<AmountOfEntity> GetAmountOfCattleInLactationPeriodAsync(Guid userId)
    {
        int amountOfCattleInLactationPeriod = await _cattleRepository.GetAmountOfCattleInLactationPeriodAsync(userId);
        return new AmountOfEntity()
        {
            Amount = amountOfCattleInLactationPeriod
        };
    }

    public async Task<AmountOfEntity> GetAmountOfCattleInDryPeriodAsync(Guid userId)
    {
        int amountNotInLactationPeriod = await _cattleRepository.GetAmountOfCattleInDryPeriodAsync(userId);
        return new AmountOfEntity()
        {
            Amount = amountNotInLactationPeriod
        };
    }

    public async Task<IEnumerable<CattleResponse>> GetCattleByNameAsync(string cattleName, Guid userId)
    {
        IEnumerable<Cattle> cattleByName = await _cattleRepository.GetCattleByName(cattleName, userId);

        List<CattleResponse> cattleResponse = new();
        foreach (Cattle cattle in cattleByName)
        {
            cattleResponse.Add(GenerateCattleResponseDto(cattle));
        }

        return cattleResponse;
    }

    public async Task<IEnumerable<DropdownData>> GetMaleCattleByName(string name, Guid userId)
    {
        if (name?.Length == 0)
            throw new BadRequestException("Nome do animal deve ser especificado.");

        string nameWithoutAccent = StringExtensions.RemoveDiacritics(name!);

        return await _cattleRepository.GetMaleCattleByName(nameWithoutAccent, userId);
    }

    public async Task<IEnumerable<DropdownData>> GetFemaleCattleByName(string name, Guid userId)
    {
        if (name?.Length == 0)
            throw new BadRequestException("Nome do animal deve ser especificado.");

        string nameWithoutAccent = StringExtensions.RemoveDiacritics(name!);

        return await _cattleRepository.GetFemaleCattleByName(nameWithoutAccent, userId);
    }

    public async Task<IEnumerable<DropdownData>> GetAllCattleByNameForDropdownAsync(Guid userId, string name)
    {
        if (name?.Length == 0)
            throw new BadRequestException("Nome do animal deve ser especificado.");

        string nameWithoutAccent = StringExtensions.RemoveDiacritics(name!);

        return await _cattleRepository.GetAllCattleByNameForDropdownAsync(nameWithoutAccent, userId);
    }

    public async Task<IEnumerable<CattleResponse>> GetAllChildrenFromCattle(Guid cattleId, Guid userId)
    {
        Cattle? cattle = await _cattleRepository.GetCattleById(cattleId, userId, trackChanges: false);
        if (cattle is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        IEnumerable<Cattle> cattleChildren =
            await _cattleRepository.GetAllChildrenFromCattleFromSpecificGenderAsync(cattleId, userId, (Gender)cattle.SexId);

        List<CattleResponse> cattleChildrenResponse = new();
        foreach (Cattle animal in cattleChildren)
        {
            cattleChildrenResponse.Add(GenerateCattleResponseDto(animal));
        }

        return cattleChildrenResponse;
    }

    public async Task<IEnumerable<CalvingInterval>> GetAllCalvingIntervalsFromCattleAsync(Guid cattleId, Guid userId)
    {
        Cattle? cattle = await _cattleRepository.GetCattleById(cattleId, userId, false);
        if (cattle is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        if ((Gender)cattle.SexId == Gender.Male)
            throw new BadRequestException("Não é possível obter intervalo entre partos de animais machos.");

        List<Cattle> allChildrenFromCattle = (List<Cattle>)await _cattleRepository.GetAllChildrenFromCattleAsync(cattleId, userId);
        List<CalvingInterval> calvingIntervals = new();
        for (int i = 0; i < allChildrenFromCattle.Count - 1; i++)
        {
            CalvingInterval calvingInterval =
                CalculateCalvingInterval(allChildrenFromCattle[i], allChildrenFromCattle[i + 1]);

            calvingIntervals.Add(calvingInterval);
        }

        return calvingIntervals;
    }

    public async Task<CattleResponse> CreateCattle(CattleRequest cattleRequest, Guid userId)
    {
        Cattle? cattleByName = await _cattleRepository.GetCattleBySpecificName(cattleRequest.Name, userId);
        if (cattleByName is not null)
            throw new ConflictException("Animal com esse nome já existe.");

        await ValidateCattleParents(cattleRequest.FatherId, cattleRequest.MotherId);

        Cattle cattleToRegister = GenerateCattleFromRequest(cattleRequest);

        GenerateListOfCattleBreedRequest(cattleRequest, cattleToRegister);
        GenerateListOfOwnerRequest(cattleRequest, cattleToRegister);

        _cattleRepository.Add(cattleToRegister);
        await _cattleRepository.CommitAsync();

        var createdCattle = await GetCattleById(cattleToRegister.Id, userId);
        if (createdCattle is null)
            throw new Exception("Não foi possível retornar os dados do animal.");

        return createdCattle;
    }

    public async Task<CattleResponse> EditCattle(EditCattleRequest cattleRequest, Guid userId, Guid routeId)
    {
        if (routeId != cattleRequest.Id)
            throw new BadRequestException("Id da rota não coincide com o id do animal especificado.");

        Cattle? cattle = await _cattleRepository.GetCattleById(cattleRequest.Id.Value, userId, false);
        if (cattle is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        Cattle? cattleByName = await _cattleRepository.GetCattleBySpecificName(cattleRequest.Name, userId);
        if (cattleByName is not null && cattleByName.Name == cattleRequest.Name && cattleByName.Id != cattleRequest.Id)
            throw new ConflictException("Animal com esse nome já existe.");

        await ValidateCattleParents(cattleRequest.FatherId, cattleRequest.MotherId, cattleRequest.Id);

        Cattle cattleToEdit = GenerateCattleFromRequest(cattleRequest);

        _cattleRepository.Update(cattleToEdit);
        await _cattleRepository.CommitAsync();

        var updatedCattle = await GetCattleById(cattleToEdit.Id, userId);
        if (updatedCattle is null)
            throw new Exception("Não foi possível retornar os dados do animal.");

        return updatedCattle;
    }

    public async Task DeleteCattle(Guid cattleId, Guid userId)
    {
        var cattle = await _cattleRepository.GetCattleById(cattleId, userId, false);
        if (cattle is null)
            throw new NotFoundException("Animal com o id especificado não foi encontrado.");

        _cattleRepository.Delete(cattle);
        await _cattleRepository.CommitAsync();
    }

    private Cattle GenerateCattleFromRequest(ICattleRequest cattleRequest)
    {
        Guid cattleId = _guidProvider.NewGuid();
        return new()
        {
            Id = cattleRequest.Id ?? cattleId,
            Name = cattleRequest.Name,
            PurchaseDate = cattleRequest.PurchaseDate,
            DateOfBirth = cattleRequest.DateOfBirth,
            YearOfBirth = cattleRequest.YearOfBirth,
            IsInLactationPeriod = cattleRequest.IsInLactationPeriod,
            Image = cattleRequest.Image,
            DateOfDeath = cattleRequest.DateOfDeath,
            CauseOfDeath = cattleRequest.CauseOfDeath,
            DateOfSale = cattleRequest.DateOfSale,
            PriceInCentsInReais = cattleRequest.PriceInCentsInReais,
            CattleOwners = new List<CattleOwner>(),
            FatherId = cattleRequest.FatherId,
            MotherId = cattleRequest.MotherId,
            SexId = cattleRequest.SexId,
            CattleBreeds = new List<CattleBreed>(),
        };
    }

    private async Task ValidateCattleParents(Guid? fatherId, Guid? motherId, Guid? cattleId = null)
    {
        if (cattleId is not null && (cattleId == fatherId || cattleId == motherId))
            throw new BadRequestException("Animal não pode ser pai ou mãe dele próprio.");

        if (fatherId is not null)
        {
            var cattleFather = await _cattleRepository.GetByIdAsync(fatherId.Value);
            if (cattleFather is null)
                throw new NotFoundException("Pai especificado não foi encontrado.");
            if (cattleFather.SexId == (byte)Gender.Female)
                throw new BadRequestException("Pai do animal não pode ser do sexo feminino.");
        }
        if (motherId is not null)
        {
            var cattleMother = await _cattleRepository.GetByIdAsync(motherId.Value);
            if (cattleMother is null)
                throw new NotFoundException("Mãe especificada não foi encontrada.");
            if (cattleMother.SexId == (byte)Gender.Male)
                throw new BadRequestException("Mãe do animal não pode ser do sexo masculino.");
        }
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
            cattle.IsInLactationPeriod,
            ((Gender)cattle.SexId).ToString(),
            cattle.CattleBreeds
            .Select(x => new CattleBreedResponse(
                x.Breed.Name, DecimalToFractionService.RealToFraction((double)x.QuantityInPercentage))),
            cattle.PurchaseDate,
            cattle.DateOfBirth,
            cattle.YearOfBirth,
            cattle.Image,
            cattle.DateOfDeath,
            cattle.CauseOfDeath,
            cattle.DateOfSale,
            cattle.CattleOwners.Select(x => new CattleOwnerResponse(x.User.FirstName, x.User.LastName))
        );
    }

    private static CalvingInterval CalculateCalvingInterval(Cattle cattle1, Cattle cattle2)
    {
        DateOnly date1 = cattle1.DateOfBirth ?? new DateOnly(cattle1.YearOfBirth, 1, 1);
        DateOnly date2 = cattle2.DateOfBirth ?? new DateOnly(cattle2.YearOfBirth, 1, 1);

        if (date2 < date1)
        {
            (date1, date2) = (date2, date1);
        }

        int years = date2.Year - date1.Year;
        int months = date2.Month - date1.Month;
        int days = date2.Day - date1.Day;

        if (days < 0)
        {
            months--;
            days += DateTime.DaysInMonth(date1.Year, date1.Month);
        }
        if (months < 0)
        {
            years--;
            months += 12;
        }

        int totalMonths = (years * 12) + months;
        int totalYears = totalMonths / 12;
        int remainingMonths = totalMonths % 12;

        return new CalvingInterval()
        {
            From = cattle1.Name,
            To = cattle2.Name,
            Years = totalYears,
            Months = remainingMonths,
            Days = days
        };
    }
}