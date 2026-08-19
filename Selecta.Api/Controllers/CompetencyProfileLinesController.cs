using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selecta.Core.Dtos;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CompetencyProfileLinesController(ICompetencyProfileLineService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompetencyProfileLineDto>>> GetAll([FromQuery] Guid? competencyProfileId, CancellationToken ct) =>
        Ok(competencyProfileId is null ? await service.GetAllAsync(ct) : await service.GetByProfileAsync(competencyProfileId.Value, ct));

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<CompetencyProfileLineDto>>> GetPaged(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 25, [FromQuery] string? filter = null, [FromQuery] string? orderBy = null, CancellationToken ct = default) =>
        Ok(await service.GetPagedAsync(page, pageSize, filter, orderBy, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CompetencyProfileLineDto>> GetById(Guid id, CancellationToken ct)
    {
        var line = await service.GetByIdAsync(id, ct);
        return line is null ? NotFound() : Ok(line);
    }

    [HttpPost]
    public async Task<ActionResult<CompetencyProfileLineDto>> Create(CreateCompetencyProfileLineDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCompetencyProfileLineDto dto, CancellationToken ct)
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
