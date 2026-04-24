using LeadFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LeadFlow.Infra.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<LeadEntity> Leads => Set<LeadEntity>();
    public DbSet<TaskItemEntity> Tasks => Set<TaskItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LeadEntity>()
            .HasMany(l => l.Tasks)
            .WithOne(t => t.Lead)
            .HasForeignKey(t => t.LeadId);

        base.OnModelCreating(modelBuilder);
    }
}