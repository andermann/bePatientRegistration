using bePatientRegistration.Application.Patients.Services;
using bePatientRegistration.Application.Patients.ServicesImpl;
using bePatientRegistration.Application.Repositories;
using bePatientRegistration.Infrastructure.Persistence;
using bePatientRegistration.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// 1) Controllers
builder.Services.AddControllers();

// 2) Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "bePatientRegistration API",
        Version = "v1",
        Description = "API para cadastro de pacientes e convênios (desafio BE3)."
    });
});

// 3) DbContext (se o Docker não estiver ok, temporariamente pode trocar para InMemory)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 4) Application Services
builder.Services.AddScoped<IPatientAppService, PatientAppService>();
builder.Services.AddScoped<IHealthPlanAppService, HealthPlanAppService>();

// 5) Repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IHealthPlanRepository, HealthPlanRepository>();

var app = builder.Build();

// 6) PIPELINE HTTP
// Deixa o Swagger SEM condicional de ambiente por enquanto,
// pra facilitar o desenvolvimento.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "bePatientRegistration API v1");
    c.RoutePrefix = "swagger"; // URL: /swagger
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
