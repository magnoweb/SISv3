using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selecta.Core.Dtos;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReportTemplatesController(IReportTemplateService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReportTemplateDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<ReportTemplateDto>>> GetPaged(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 25, [FromQuery] string? filter = null, [FromQuery] string? orderBy = null, CancellationToken ct = default) =>
        Ok(await service.GetPagedAsync(page, pageSize, filter, orderBy, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReportTemplateDto>> GetById(int id, CancellationToken ct)
    {
        var template = await service.GetByIdAsync(id, ct);
        return template is null ? NotFound() : Ok(template);
    }

    [HttpPost]
    public async Task<ActionResult<ReportTemplateDto>> Create(CreateReportTemplateDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateReportTemplateDto dto, CancellationToken ct)
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
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
