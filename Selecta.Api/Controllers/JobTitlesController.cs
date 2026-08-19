using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selecta.Core.Dtos;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class JobTitlesController(IJobTitleService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobTitleDto>>> GetAll([FromQuery] Guid? companyId, CancellationToken ct) =>
        Ok(companyId is null ? await service.GetAllAsync(ct) : await service.GetByCompanyAsync(companyId.Value, ct));

    /// <summary>Listagem paginada (usada pela tela de listagem) — GetAll continua disponível para dropdowns.</summary>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<JobTitleDto>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 25, [FromQuery] string? filter = null, [FromQuery] string? orderBy = null, CancellationToken ct = default) =>
        Ok(await service.GetPagedAsync(page, pageSize, filter, orderBy, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobTitleDto>> GetById(Guid id, CancellationToken ct)
    {
        var jobTitle = await service.GetByIdAsync(id, ct);
        return jobTitle is null ? NotFound() : Ok(jobTitle);
    }

    [HttpPost]
    public async Task<ActionResult<JobTitleDto>> Create(CreateJobTitleDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateJobTitleDto dto, CancellationToken ct)
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
