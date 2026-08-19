using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selecta.Core.Dtos;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RecruitmentStagesController(IRecruitmentStageService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecruitmentStageDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));

    /// <summary>Listagem paginada (usada pela tela de listagem) — GetAll continua disponível para dropdowns.</summary>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<RecruitmentStageDto>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 25, [FromQuery] string? filter = null, [FromQuery] string? orderBy = null, CancellationToken ct = default) =>
        Ok(await service.GetPagedAsync(page, pageSize, filter, orderBy, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecruitmentStageDto>> GetById(Guid id, CancellationToken ct)
    {
        var stage = await service.GetByIdAsync(id, ct);
        return stage is null ? NotFound() : Ok(stage);
    }

    [HttpPost]
    public async Task<ActionResult<RecruitmentStageDto>> Create(CreateRecruitmentStageDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateRecruitmentStageDto dto, CancellationToken ct)
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
