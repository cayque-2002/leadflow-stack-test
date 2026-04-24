using LeadFlow.Application.Dtos.Tasks;

namespace LeadFlow.Application.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>?> GetByLeadAsync(int leadId);
    Task<TaskDto?> GetByIdAsync(int leadId, int taskId);
    Task<TaskDto?> CreateAsync(int leadId, TaskCreateDto dto);
    Task<bool> UpdateAsync(int leadId, int taskId, TaskUpdateDto dto);
    Task<bool> DeleteAsync(int leadId, int taskId);
}