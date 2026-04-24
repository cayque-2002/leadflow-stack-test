using LeadFlow.Application.Dtos.Tasks;
using LeadFlow.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeadFlow.Api.Controllers;

[ApiController]
[Route("api/leads/{leadId:int}/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetByLead(int leadId)
    {
        var tasks = await _taskService.GetByLeadAsync(leadId);

        if (tasks is null)
            return NotFound(new { message = "Lead não encontrado." });

        return Ok(tasks);
    }

    [HttpGet("{taskId:int}")]
    public async Task<ActionResult<TaskDto>> GetById(int leadId, int taskId)
    {
        var task = await _taskService.GetByIdAsync(leadId, taskId);

        if (task is null)
            return NotFound(new { message = "Task não encontrada." });

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create(int leadId, TaskCreateDto dto)
    {
        try
        {
            var task = await _taskService.CreateAsync(leadId, dto);

            if (task is null)
                return NotFound(new { message = "Lead não encontrado." });

            return CreatedAtAction(
                nameof(GetById),
                new { leadId, taskId = task.Id },
                task
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{taskId:int}")]
    public async Task<IActionResult> Update(int leadId, int taskId, TaskUpdateDto dto)
    {
        try
        {
            var updated = await _taskService.UpdateAsync(leadId, taskId, dto);

            if (!updated)
                return NotFound(new { message = "Task não encontrada." });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> Delete(int leadId, int taskId)
    {
        var deleted = await _taskService.DeleteAsync(leadId, taskId);

        if (!deleted)
            return NotFound(new { message = "Task não encontrada." });

        return NoContent();
    }
}