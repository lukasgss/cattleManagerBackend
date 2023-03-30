using AutoMapper;
using CatetleManager.Application.Domain.Entities;
using CattleManager.Application.Application.Common.Enums;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Services.Entities;

public class ConceptionService : IConceptionService
{
    private readonly IConceptionRepository _conceptionRepository;
    private readonly ICattleRepository _cattleRepository;
    private readonly IMapper _mapper;

    public ConceptionService(IConceptionRepository conceptionRepository, ICattleRepository cattleRepository, IMapper mapper)
    {
        _conceptionRepository = conceptionRepository;
        _cattleRepository = cattleRepository;
        _mapper = mapper;
    }

    public async Task<ConceptionResponse> GetConceptionByIdAsync(Guid conceptionId)
    {
        Conception? conception = await _conceptionRepository.GetByIdAsync(conceptionId);
        if (conception is null)
            throw new NotFoundException("Concepção com o id especificado não existe.");

        return _mapper.Map<ConceptionResponse>(conception);
    }

    public async Task<IEnumerable<ConceptionResponse>> GetAllConceptionsFromCattleAsync(Guid cattleId, Guid userId, int page)
    {
        Cattle? cattleToQuery = await _cattleRepository.GetCattleById(cattleId, userId, false);
        if (cattleToQuery is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        var conceptions = await _conceptionRepository.GetAllConceptionsFromCattle(cattleId, userId, page);
        return _mapper.Map<List<ConceptionResponse>>(conceptions);
    }

    public async Task<ConceptionResponse> CreateConceptionAsync(CreateConceptionRequest conceptionRequest, Guid userId)
    {
        await ValidateCattleParents(conceptionRequest.MotherId, conceptionRequest.FatherId, userId);

        Conception conception = _mapper.Map<Conception>(conceptionRequest);
        _conceptionRepository.Add(conception);
        await _conceptionRepository.CommitAsync();

        return _mapper.Map<ConceptionResponse>(conception);
    }

    public async Task<ConceptionResponse> EditConceptionAsync(EditConceptionRequest conceptionRequest, Guid userId, Guid routeId)
    {
        if (conceptionRequest.Id != routeId)
            throw new BadRequestException("Rota não coincide com o id especificado.");

        Conception? conceptionToEdit = await _conceptionRepository.GetConceptionByIdAsync(conceptionRequest.Id, false);
        if (conceptionToEdit is null)
            throw new NotFoundException("Concepção com o id especificado não foi encontrada.");

        await ValidateCattleParents(conceptionRequest.MotherId, conceptionRequest.FatherId, userId);

        Conception editedConception = _mapper.Map<Conception>(conceptionRequest);
        _conceptionRepository.Update(editedConception);
        await _conceptionRepository.CommitAsync();

        return _mapper.Map<ConceptionResponse>(editedConception);
    }

    public async Task DeleteConceptionAsync(Guid conceptionId, Guid userId)
    {
        Conception? conceptionToDelete = await _conceptionRepository.GetByIdAsync(conceptionId);
        if (conceptionToDelete is null)
            throw new NotFoundException("Concepção com o id especificado não foi encontrada.");

        _conceptionRepository.Delete(conceptionToDelete);
        await _conceptionRepository.CommitAsync();
    }

    private async Task ValidateCattleParents(Guid motherId, Guid fatherId, Guid userId)
    {
        Cattle? cattleMother = await _cattleRepository.GetCattleById(motherId, userId, true);
        if (cattleMother?.Users.Any(x => x.Id == userId) != true)
            throw new NotFoundException("Animal com o id especificado não existe.");
        if (cattleMother?.SexId == (int)Gender.Male)
            throw new BadRequestException("Mãe não pode ser do sexo masculino.");

        Cattle? cattleFather = await _cattleRepository.GetCattleById(fatherId, userId, true);
        if (cattleFather?.Users.Any(x => x.Id == userId) != true)
            throw new NotFoundException("Animal com o id especificado não existe.");
        if (cattleFather.SexId == (int)Gender.Female)
            throw new BadRequestException("Pai não pode ser do sexo feminino.");
    }
}