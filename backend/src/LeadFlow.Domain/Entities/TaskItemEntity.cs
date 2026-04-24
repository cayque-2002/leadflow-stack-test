using LeadFlow.Domain.Enums;

namespace LeadFlow.Domain.Entities;

public class TaskItemEntity
{
    public int Id { get; set; }
    public int LeadId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public LeadEntity? Lead { get; set; }
}