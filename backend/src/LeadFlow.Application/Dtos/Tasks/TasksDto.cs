using LeadFlow.Domain.Enums;

namespace LeadFlow.Application.Dtos.Tasks;

public record TaskCreateDto(string Title, DateTime? DueDate, TaskItemStatus? Status);
public record TaskUpdateDto(string Title, DateTime? DueDate, TaskItemStatus Status);
public record TaskDto(int Id, int LeadId, string Title, DateTime? DueDate, TaskItemStatus Status, DateTime CreatedAt, DateTime UpdatedAt);
