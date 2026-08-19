using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selecta.Core.Dtos;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AssessmentEventTestsController(IAssessmentEventTestService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssessmentEventTestDto>>> GetAll([FromQuery] Guid? assessmentEventId, CancellationToken ct) =>
        Ok(assessmentEventId is null ? await service.GetAllAsync(ct) : await service.GetByAssessmentEventAsync(assessmentEventId.Value, ct));

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<AssessmentEventTestDto>>> GetPaged(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 25, [FromQuery] string? filter = null, [FromQuery] string? orderBy = null, CancellationToken ct = default) =>
        Ok(await service.GetPagedAsync(page, pageSize, filter, orderBy, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AssessmentEventTestDto>> GetById(Guid id, CancellationToken ct)
    {
        var test = await service.GetByIdAsync(id, ct);
        return test is null ? NotFound() : Ok(test);
    }

    [HttpPost]
    public async Task<ActionResult<AssessmentEventTestDto>> Create(CreateAssessmentEventTestDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateAssessmentEventTestDto dto, CancellationToken ct)
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
