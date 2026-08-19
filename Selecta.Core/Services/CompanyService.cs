using Selecta.Core.Dtos;
using Selecta.Core.Entities.Common;
using Selecta.Core.Exceptions;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class CompanyService(ICompanyRepository repository) : ICompanyService
{
    public async Task<IEnumerable<CompanyDto>> GetAllAsync(CancellationToken ct = default)
    {
        var companies = await repository.GetAllAsync(ct);
        return companies.Select(ToDto);
    }

    public async Task<PagedResult<CompanyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<CompanyDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<CompanyDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var company = await repository.GetByIdAsync(id, ct);
        return company is null ? null : ToDto(company);
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto, CancellationToken ct = default)
    {
        await EnsureDocumentIsUniqueAsync(dto.Document, currentCompanyId: null, ct);

        var company = new Company
        {
            Type = dto.Type,
            LegalName = dto.LegalName,
            TradeName = dto.TradeName,
            Document = dto.Document,
            StateRegistration = dto.StateRegistration,
            Address = dto.Address,
            AddressComplement = dto.AddressComplement,
            Neighborhood = dto.Neighborhood,
            CityName = dto.CityName,
            State = dto.State,
            PostalCode = dto.PostalCode,
            Phone1 = dto.Phone1,
            Phone2 = dto.Phone2,
            Notes = dto.Notes,
            CityId = dto.CityId,
        };

        await repository.AddAsync(company, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(company);
    }

    public async Task UpdateAsync(UpdateCompanyDto dto, CancellationToken ct = default)
    {
        var company = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"Company {dto.Id} not found.");

        await EnsureDocumentIsUniqueAsync(dto.Document, currentCompanyId: dto.Id, ct);

        company.Type = dto.Type;
        company.LegalName = dto.LegalName;
        company.TradeName = dto.TradeName;
        company.Document = dto.Document;
        company.StateRegistration = dto.StateRegistration;
        company.Address = dto.Address;
        company.AddressComplement = dto.AddressComplement;
        company.Neighborhood = dto.Neighborhood;
        company.CityName = dto.CityName;
        company.State = dto.State;
        company.PostalCode = dto.PostalCode;
        company.Phone1 = dto.Phone1;
        company.Phone2 = dto.Phone2;
        company.Notes = dto.Notes;
        company.Active = dto.Active;
        company.CityId = dto.CityId;

        repository.Update(company);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var company = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Company {id} not found.");

        repository.Remove(company);
        await repository.SaveChangesAsync(ct);
    }

    private async Task EnsureDocumentIsUniqueAsync(string document, Guid? currentCompanyId, CancellationToken ct)
    {
        var existing = await repository.GetByDocumentAsync(document, ct);
        if (existing is not null && existing.Id != currentCompanyId)
            throw new DomainException("Já existe uma empresa com este documento.");
    }

    private static CompanyDto ToDto(Company c) => new(
        c.Id, c.Type, c.LegalName, c.TradeName, c.Document, c.StateRegistration, c.Address,
        c.AddressComplement, c.Neighborhood, c.CityName, c.State, c.PostalCode, c.Phone1, c.Phone2,
        c.Notes, c.Active, c.CreatedAt, c.CityId);
}
