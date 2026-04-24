using LeadFlow.Application.Dtos.Leads;
using LeadFlow.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeadFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly ILeadService _leadService;

    public LeadsController(ILeadService leadService)
    {
        _leadService = leadService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeadDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? status)
    {
        var leads = await _leadService.GetAllAsync(search, status);
        return Ok(leads);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LeadDto>> GetById(int id)
    {
        var lead = await _leadService.GetByIdAsync(id);

        if (lead is null)
            return NotFound(new { message = "Lead não encontrado." });

        return Ok(lead);
    }

    [HttpPost]
    public async Task<ActionResult<LeadDto>> Create(LeadCreateDto dto)
    {
        try
        {
            var lead = await _leadService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = lead.Id },
                lead
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, LeadUpdateDto dto)
    {
        try
        {
            var updated = await _leadService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = "Lead não encontrado." });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _leadService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Lead não encontrado." });

        return NoContent();
    }
}