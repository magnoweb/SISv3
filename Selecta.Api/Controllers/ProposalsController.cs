using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selecta.Core.Dtos;
using Selecta.Core.Exceptions;
using Selecta.Core.Interfaces.Services;

namespace Selecta.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProposalsController(IProposalService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProposalDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<ProposalDto>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 25, [FromQuery] string? filter = null, [FromQuery] string? orderBy = null, CancellationToken ct = default) =>
        Ok(await service.GetPagedAsync(page, pageSize, filter, orderBy, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProposalDto>> GetById(Guid id, CancellationToken ct)
    {
        var proposal = await service.GetByIdAsync(id, ct);
        return proposal is null ? NotFound() : Ok(proposal);
    }

    [HttpPost]
    public async Task<ActionResult<ProposalDto>> Create(CreateProposalDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProposalDto dto, CancellationToken ct)
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

    /// <summary>Troca de status — exige motivo de recusa quando NewStatus == Declined.</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, ChangeProposalStatusDto dto, CancellationToken ct)
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
