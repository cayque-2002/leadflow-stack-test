using LeadFlow.Application.Dtos.Leads;

namespace LeadFlow.Application.Services;

public interface ILeadService
{
    Task<IEnumerable<LeadDto>> GetAllAsync(string? search, int? status);
    Task<LeadDto?> GetByIdAsync(int id);
    Task<LeadDto> CreateAsync(LeadCreateDto dto);
    Task<bool> UpdateAsync(int id, LeadUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}