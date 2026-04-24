using LeadFlow.Infra.Data;
using LeadFlow.Application.Services;
using Microsoft.EntityFrameworkCore;
using LeadFlow.Application.Services;
using LeadFlow.Infra.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=leadflow.db"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();