using Selecta.Core.Dtos;
using Selecta.Core.Entities.Common;
using Selecta.Core.Interfaces.Repositories;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Core.Services;

public class ContactService(IContactRepository repository) : IContactService
{
    public async Task<IEnumerable<ContactDto>> GetAllAsync(CancellationToken ct = default)
    {
        var contacts = await repository.GetAllAsync(ct);
        return contacts.Select(ToDto);
    }

    public async Task<PagedResult<ContactDto>> GetPagedAsync(int page, int pageSize, string? filter = null, string? orderBy = null, CancellationToken ct = default)
    {
        var result = await repository.GetPagedAsync(page, pageSize, filter, orderBy, ct);
        return new PagedResult<ContactDto>(result.Items.Select(ToDto).ToList(), result.TotalCount, result.Page, result.PageSize);
    }

    public async Task<IEnumerable<ContactDto>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default)
    {
        var contacts = await repository.GetByCompanyAsync(companyId, ct);
        return contacts.Select(ToDto);
    }

    public async Task<ContactDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var contact = await repository.GetByIdAsync(id, ct);
        return contact is null ? null : ToDto(contact);
    }

    public async Task<ContactDto> CreateAsync(CreateContactDto dto, CancellationToken ct = default)
    {
        var contact = new Contact
        {
            CompanyId = dto.CompanyId,
            Name = dto.Name,
            Gender = dto.Gender,
            Position = dto.Position,
            Phone1 = dto.Phone1,
            Phone2 = dto.Phone2,
            Email = dto.Email,
            BirthDay = dto.BirthDay,
            BirthMonth = dto.BirthMonth,
            Notes = dto.Notes,
            ReceiveNotifications = dto.ReceiveNotifications,
        };

        await repository.AddAsync(contact, ct);
        await repository.SaveChangesAsync(ct);
        return ToDto(contact);
    }

    public async Task UpdateAsync(UpdateContactDto dto, CancellationToken ct = default)
    {
        var contact = await repository.GetByIdAsync(dto.Id, ct)
            ?? throw new KeyNotFoundException($"Contact {dto.Id} not found.");

        contact.CompanyId = dto.CompanyId;
        contact.Name = dto.Name;
        contact.Gender = dto.Gender;
        contact.Position = dto.Position;
        contact.Phone1 = dto.Phone1;
        contact.Phone2 = dto.Phone2;
        contact.Email = dto.Email;
        contact.BirthDay = dto.BirthDay;
        contact.BirthMonth = dto.BirthMonth;
        contact.Notes = dto.Notes;
        contact.ReceiveNotifications = dto.ReceiveNotifications;
        contact.Active = dto.Active;

        repository.Update(contact);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var contact = await repository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Contact {id} not found.");

        repository.Remove(contact);
        await repository.SaveChangesAsync(ct);
    }

    private static ContactDto ToDto(Contact c) => new(
        c.Id, c.CompanyId, c.Name, c.Gender, c.Position, c.Phone1, c.Phone2, c.Email,
        c.BirthDay, c.BirthMonth, c.Notes, c.ReceiveNotifications, c.Active, c.CreatedAt);
}
