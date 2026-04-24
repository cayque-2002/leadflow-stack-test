using LeadFlow.Domain.Enums;

namespace LeadFlow.Domain.Entities;

public class LeadEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public LeadStatus Status { get; set; } = LeadStatus.New;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TaskItemEntity> Tasks { get; set; } = new List<TaskItemEntity>();
}