using Selecta.Core.Dtos;
using Selecta.Core.Entities.Administrative;
using Selecta.Core.Exceptions;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ProspectCompanyService(IProspectCompanyRepository repository) : IProspectCompanyService
{
    public async Task<IEnumerable<ProspectCompanyDto>> GetAllAsync(CancellationToken ct = default)
    {
        var companies = await repository.GetAllAsync(ct);
        return companies.Select(ToDto);
    }

    public async Task<PagedResult<ProspectCompanyDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ProspectCompanyDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<ProspectCompanyDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var company = await repository.GetByIdAsync(id, ct);
        return company is null ? null : ToDto(company);
    }

    public async Task<ProspectCompanyDto> CreateAsync(CreateProspectCompanyDto dto, CancellationToken ct = default)
    {
        await EnsureDocumentIsUniqueAsync(dto.Document, currentId: null, ct);

        var company = new ProspectCompany { CompanyId = dto.CompanyId, Name = dto.Name, Document = dto.Document };
        await repository.AddAsync(company, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(company);
    }

    public async Task UpdateAsync(UpdateProspectCompanyDto dto, CancellationToken ct = default)
    {
        var company = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"ProspectCompany {dto.Id} not found.");

        await EnsureDocumentIsUniqueAsync(dto.Document, currentId: dto.Id, ct);

        company.CompanyId = dto.CompanyId;
        company.Name = dto.Name;
        company.Document = dto.Document;

        repository.Update(company);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var company = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"ProspectCompany {id} not found.");

        repository.Remove(company);
        await repository.SaveChangesAsync(ct);
    }

    private async Task EnsureDocumentIsUniqueAsync(string document, Guid? currentId, CancellationToken ct)
    {
        var existing = await repository.GetByDocumentAsync(document, ct);
        if (existing is not null && existing.Id != currentId)
            throw new DomainException("Já existe um registo com este documento.");
    }

    private static ProspectCompanyDto ToDto(ProspectCompany c) => new(c.Id, c.CompanyId, c.Name, c.Document);
}
