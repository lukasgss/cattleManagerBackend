using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;
using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccines;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Services.Entities;

public class VaccinationService : IVaccinationService
{
    private readonly IVaccinationRepository _vaccinationRepository;
    private readonly ICattleRepository _cattleRepository;
    private readonly IVaccineRepository _vaccineRepository;
    private readonly IMapper _mapper;
    public VaccinationService(
        IVaccinationRepository vaccinationRepository,
        IVaccineRepository vaccineRepository,
        ICattleRepository cattleRepository,
        IMapper mapper)
    {
        _vaccinationRepository = vaccinationRepository;
        _cattleRepository = cattleRepository;
        _vaccineRepository = vaccineRepository;
        _mapper = mapper;
    }

    public async Task<VaccinationResponse> GetVaccinationByIdAsync(Guid vaccinationId)
    {
        Vaccination? vaccination = await _vaccinationRepository.GetVaccinationByIdAsync(vaccinationId);
        if (vaccination is null)
            throw new NotFoundException("Vacinação com o id especificado não existe.");

        return _mapper.Map<VaccinationResponse>(vaccination);
    }

    public async Task<PaginatedVaccinationResponse> GetAllVaccinationsFromCattle(Guid cattleId, Guid userId, int page)
    {
        double amountOfPages = _vaccinationRepository.GetAmountOfPages(cattleId, userId);
        if ((page > amountOfPages && amountOfPages > 1) || page < 1)
            throw new BadRequestException($"Resultado possui {amountOfPages} página(s), insira um valor entre 1 e o número de páginas.");

        IEnumerable<Vaccination> cattleVaccinations = await _vaccinationRepository.GetAllVaccinationsFromCattle(cattleId, userId, page);
        List<VaccinationResponse> vaccinationResponse = _mapper.Map<List<VaccinationResponse>>(cattleVaccinations);

        return new PaginatedVaccinationResponse(vaccinationResponse, page, amountOfPages);
    }

    public async Task<VaccinationResponse> CreateVaccinationAsync(CreateVaccinationRequest vaccinationRequest, Guid userId)
    {
        Cattle? vaccinatedCattle = await _cattleRepository.GetCattleById(vaccinationRequest.CattleId, userId, true);
        if (vaccinatedCattle is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        Vaccine? givenVaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccinationRequest.VaccineId);
        if (givenVaccine is null)
            throw new NotFoundException("Vacina com o id especificado não existe.");

        Vaccination vaccination = _mapper.Map<Vaccination>(vaccinationRequest);

        _vaccinationRepository.Add(vaccination);
        await _vaccinationRepository.CommitAsync();

        return _mapper.Map<VaccinationResponse>(vaccination);
    }

    public async Task<VaccinationResponse> EditVaccinationAsync(EditVaccinationRequest vaccinationRequest, Guid routeId, Guid userId)
    {
        if (routeId != vaccinationRequest.Id)
            throw new BadRequestException("Rota não coincide com o id da vacinação.");

        Vaccination? vaccinationToEdit = await _vaccinationRepository.GetVaccinationByIdAsync(vaccinationRequest.Id);
        if (vaccinationToEdit is null)
            throw new NotFoundException("Vacinação com o id especificado não existe.");

        Cattle? vaccinatedCattle = await _cattleRepository.GetByIdAsync(vaccinationRequest.CattleId);
        if (vaccinatedCattle is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        Vaccine? appliedVaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccinationRequest.VaccineId);
        if (appliedVaccine is null)
            throw new NotFoundException("Vacina com o id especificado não existe.");

        Vaccination editedVaccination = _mapper.Map<Vaccination>(vaccinationRequest);
        _vaccinationRepository.Update(editedVaccination);
        await _vaccinationRepository.CommitAsync();

        return _mapper.Map<VaccinationResponse>(editedVaccination);
    }

    public async Task DeleteVaccinationAsync(Guid vaccinationId, Guid userId)
    {
        Vaccination? vaccinationToDelete = await _vaccinationRepository.GetByIdAsync(vaccinationId);
        if (vaccinationToDelete is null)
            throw new NotFoundException("Vacinação com o id especificado não existe.");

        Cattle? cattleToBeVaccinated = await _cattleRepository.GetCattleById(vaccinationToDelete.CattleId, userId, false);
        if (cattleToBeVaccinated is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        _vaccinationRepository.Delete(vaccinationToDelete);
        await _vaccinationRepository.CommitAsync();
    }
}