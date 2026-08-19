using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selecta.Core.Dtos;
using Selecta.Core.Exceptions;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CandidatesController(ICandidateService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CandidateDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));

    /// <summary>Listagem paginada (usada pela tela de listagem) — GetAll continua disponível para dropdowns.</summary>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<CandidateDto>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 25, [FromQuery] string? filter = null, [FromQuery] string? orderBy = null, CancellationToken ct = default) =>
        Ok(await service.GetPagedAsync(page, pageSize, filter, orderBy, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CandidateDto>> GetById(Guid id, CancellationToken ct)
    {
        var candidate = await service.GetByIdAsync(id, ct);
        return candidate is null ? NotFound() : Ok(candidate);
    }

    /// <summary>Busca única por nome ou CPF — deteta automaticamente qual dos dois foi informado.</summary>
    [HttpGet("search")]
    public async Task<ActionResult<CandidateDto>> Search([FromQuery] string term, CancellationToken ct)
    {
        var candidate = await service.SearchByNameOrCpfAsync(term, ct);
        return candidate is null ? NotFound() : Ok(candidate);
    }

    [HttpPost]
    public async Task<ActionResult<CandidateDto>> Create(CreateCandidateDto dto, CancellationToken ct)
    {
        try
        {
            var created = await service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCandidateDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest();

        try
        {
            await service.UpdateAsync(dto, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await service.DeleteAsync(id, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
