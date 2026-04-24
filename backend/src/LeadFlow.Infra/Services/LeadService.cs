using LeadFlow.Application.Dtos.Leads;
using LeadFlow.Application.Services;
using LeadFlow.Domain.Entities;
using LeadFlow.Domain.Enums;
using LeadFlow.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LeadFlow.Infra.Services;

public class LeadService : ILeadService
{
    private readonly AppDbContext _context;

    public LeadService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LeadDto>> GetAllAsync(string? search, int? status)
    {
        var query = _context.Leads
            .Include(l => l.Tasks)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.ToLower();

            query = query.Where(l =>
                l.Name.ToLower().Contains(term) ||
                l.Email.ToLower().Contains(term)
            );
        }

        if (status.HasValue)
        {
            query = query.Where(l => (int)l.Status == status.Value);
        }

        return await query
            .Select(l => new LeadDto(
                l.Id,
                l.Name,
                l.Email,
                l.Status,
                l.CreatedAt,
                l.UpdatedAt,
                l.Tasks.Count
            ))
            .ToListAsync();
    }

    public async Task<LeadDto?> GetByIdAsync(int id)
    {
        var lead = await _context.Leads
            .Include(l => l.Tasks)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lead is null)
            return null;

        return new LeadDto(
            lead.Id,
            lead.Name,
            lead.Email,
            lead.Status,
            lead.CreatedAt,
            lead.UpdatedAt,
            lead.Tasks.Count
        );
    }

    public async Task<LeadDto> CreateAsync(LeadCreateDto dto)
    {

        var validationError = ValidateLead(dto.Name, dto.Email);

        if (validationError is not null)
            throw new ArgumentException(validationError);

        var lead = new LeadEntity
        {
            Name = dto.Name,
            Email = dto.Email,
            Status = dto.Status ?? LeadStatus.New,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Leads.Add(lead);
        await _context.SaveChangesAsync();

        return new LeadDto(
            lead.Id,
            lead.Name,
            lead.Email,
            lead.Status,
            lead.CreatedAt,
            lead.UpdatedAt,
            0
        );
    }

    public async Task<bool> UpdateAsync(int id, LeadUpdateDto dto)
    {
        var lead = await _context.Leads.FindAsync(id);

        if (lead is null)
            return false;

        var validationError = ValidateLead(dto.Name, dto.Email);

        if (validationError is not null)
            throw new ArgumentException(validationError);

        lead.Name = dto.Name;
        lead.Email = dto.Email;
        lead.Status = dto.Status;
        lead.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var lead = await _context.Leads.FindAsync(id);

        if (lead is null)
            return false;

        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();

        return true;
    }

    private static string? ValidateLead(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 3)
            return "O nome deve ter pelo menos 3 caracteres.";

        if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            return "E-mail inválido.";

        return null;
    }

}