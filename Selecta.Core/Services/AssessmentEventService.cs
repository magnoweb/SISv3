using Selecta.Core.Dtos;
using Selecta.Core.Entities.Selection;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class AssessmentEventService(IAssessmentEventRepository repository) : IAssessmentEventService
{
    public async Task<IEnumerable<AssessmentEventDto>> GetAllAsync(CancellationToken ct = default)
    {
        var events = await repository.GetAllAsync(ct);
        return events.Select(ToDto);
    }

    public async Task<PagedResult<AssessmentEventDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<AssessmentEventDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<AssessmentEventDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var evt = await repository.GetByIdAsync(id, ct);
        return evt is null ? null : ToDto(evt);
    }

    public async Task<AssessmentEventDto> CreateAsync(CreateAssessmentEventDto dto, CancellationToken ct = default)
    {
        var evt = new AssessmentEvent
        {
            CandidateId = dto.CandidateId,
            JobTitleId = dto.JobTitleId,
            ContactId = dto.ContactId,
            Date = dto.Date,
            EducationLevel = dto.EducationLevel,
            Education = dto.Education,
            EducationCompleted = dto.EducationCompleted,
            MaritalStatus = dto.MaritalStatus,
            DriverLicenseNumber = dto.DriverLicenseNumber,
            DriverLicenseCategory = dto.DriverLicenseCategory,
            NumberOfChildren = dto.NumberOfChildren,
            Address = dto.Address,
            AddressComplement = dto.AddressComplement,
            Neighborhood = dto.Neighborhood,
            CityName = dto.CityName,
            State = dto.State,
            PostalCode = dto.PostalCode,
            Phone1 = dto.Phone1,
            Phone2 = dto.Phone2,
            Email = dto.Email,
            CityId = dto.CityId,
            Purpose = dto.Purpose,
        };

        await repository.AddAsync(evt, ct);
        await repository.SaveChangesAsync(ct);

        // Recarrega com as relações incluídas, igual ao padrão de JobOpeningService.CreateAsync.
        var created = await repository.GetByIdAsync(evt.Id, ct) ?? evt;
        return ToDto(created);
    }

    public async Task UpdateAsync(UpdateAssessmentEventDto dto, CancellationToken ct = default)
    {
        var evt = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"AssessmentEvent {dto.Id} not found.");

        evt.CandidateId = dto.CandidateId;
        evt.JobTitleId = dto.JobTitleId;
        evt.ContactId = dto.ContactId;
        evt.Date = dto.Date;
        evt.EducationLevel = dto.EducationLevel;
        evt.Education = dto.Education;
        evt.EducationCompleted = dto.EducationCompleted;
        evt.MaritalStatus = dto.MaritalStatus;
        evt.DriverLicenseNumber = dto.DriverLicenseNumber;
        evt.DriverLicenseCategory = dto.DriverLicenseCategory;
        evt.NumberOfChildren = dto.NumberOfChildren;
        evt.Address = dto.Address;
        evt.AddressComplement = dto.AddressComplement;
        evt.Neighborhood = dto.Neighborhood;
        evt.CityName = dto.CityName;
        evt.State = dto.State;
        evt.PostalCode = dto.PostalCode;
        evt.Phone1 = dto.Phone1;
        evt.Phone2 = dto.Phone2;
        evt.Email = dto.Email;
        evt.CityId = dto.CityId;
        evt.Result = dto.Result;
        evt.EvaluationResultId = dto.EvaluationResultId;
        evt.Status = dto.Status;
        evt.Purpose = dto.Purpose;
        evt.CompletedAt = dto.CompletedAt;

        repository.Update(evt);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var evt = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"AssessmentEvent {id} not found.");

        repository.Remove(evt);
        await repository.SaveChangesAsync(ct);
    }

    private static AssessmentEventDto ToDto(AssessmentEvent e) => new(
        e.Id,
        e.CandidateId, e.Candidate?.Name ?? "(unknown)",
        e.JobTitleId, e.JobTitle?.Name ?? "(unknown)",
        e.ContactId, e.Contact?.Name,
        e.Date, e.EducationLevel, e.Education, e.EducationCompleted, e.MaritalStatus,
        e.DriverLicenseNumber, e.DriverLicenseCategory, e.NumberOfChildren,
        e.Address, e.AddressComplement, e.Neighborhood, e.CityName, e.State, e.PostalCode,
        e.Phone1, e.Phone2, e.Email, e.CityId,
        e.Result, e.EvaluationResultId, e.EvaluationResult?.Name,
        e.Status, e.Purpose, e.CreatedAt, e.CompletedAt);
}
