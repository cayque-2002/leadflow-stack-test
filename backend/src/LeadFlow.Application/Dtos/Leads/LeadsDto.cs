using LeadFlow.Domain.Enums;

namespace LeadFlow.Application.Dtos.Leads;

public record LeadCreateDto(string Name, string Email, LeadStatus? Status);
public record LeadUpdateDto(string Name, string Email, LeadStatus Status);
public record LeadDto(int Id, string Name, string Email, LeadStatus Status, DateTime CreatedAt, DateTime UpdatedAt, int TasksCount);