using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selecta.Core.Dtos;
using Selecta.Core.Exceptions;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class JobOpeningsController(IJobOpeningService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobOpeningDto>>> GetAll([FromQuery] bool activeOnly, CancellationToken ct) =>
        Ok(activeOnly ? await service.GetActiveAsync(ct) : await service.GetAllAsync(ct));

    /// <summary>Listagem paginada, mais recentes primeiro (usada pela tela de listagem).</summary>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<JobOpeningDto>>> GetPaged(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 25, [FromQuery] bool activeOnly = false,
        [FromQuery] string? filter = null, [FromQuery] string? orderBy = null, CancellationToken ct = default) =>
        Ok(await service.GetPagedAsync(page, pageSize, activeOnly, filter, orderBy, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobOpeningDto>> GetById(Guid id, CancellationToken ct)
    {
        var job = await service.GetByIdAsync(id, ct);
        return job is null ? NotFound() : Ok(job);
    }

    [HttpPost]
    public async Task<ActionResult<JobOpeningDto>> Create(CreateJobOpeningDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateJobOpeningDto dto, CancellationToken ct)
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
    }

    /// <summary>Troca de status com validação da máquina de estados (ver JobOpeningStatusRules).</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, ChangeJobOpeningStatusDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest();

        try
        {
            await service.ChangeStatusAsync(dto, ct);
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
