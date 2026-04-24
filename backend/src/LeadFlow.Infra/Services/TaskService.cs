using LeadFlow.Application.Dtos.Tasks;
using LeadFlow.Application.Services;
using LeadFlow.Domain.Entities;
using LeadFlow.Domain.Enums;
using LeadFlow.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace LeadFlow.Infra.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskDto>?> GetByLeadAsync(int leadId)
    {
        var leadExists = await _context.Leads.AnyAsync(l => l.Id == leadId);

        if (!leadExists)
            return null;

        return await _context.Tasks
            .Where(t => t.LeadId == leadId)
            .Select(t => new TaskDto(
                t.Id,
                t.LeadId,
                t.Title,
                t.DueDate,
                t.Status,
                t.CreatedAt,
                t.UpdatedAt
            ))
            .ToListAsync();
    }

    public async Task<TaskDto?> GetByIdAsync(int leadId, int taskId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.LeadId == leadId);

        if (task is null)
            return null;

        return new TaskDto(
            task.Id,
            task.LeadId,
            task.Title,
            task.DueDate,
            task.Status,
            task.CreatedAt,
            task.UpdatedAt
        );
    }

    public async Task<TaskDto?> CreateAsync(int leadId, TaskCreateDto dto)
    {
        var leadExists = await _context.Leads.AnyAsync(l => l.Id == leadId);

        if (!leadExists)
            return null;

        var task = new TaskItemEntity
        {
            LeadId = leadId,
            Title = dto.Title,
            DueDate = dto.DueDate,
            Status = dto.Status ?? TaskItemStatus.Todo,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return new TaskDto(
            task.Id,
            task.LeadId,
            task.Title,
            task.DueDate,
            task.Status,
            task.CreatedAt,
            task.UpdatedAt
        );
    }

    public async Task<bool> UpdateAsync(int leadId, int taskId, TaskUpdateDto dto)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.LeadId == leadId);

        if (task is null)
            return false;

        task.Title = dto.Title;
        task.DueDate = dto.DueDate;
        task.Status = dto.Status;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int leadId, int taskId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.LeadId == leadId);

        if (task is null)
            return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }
}