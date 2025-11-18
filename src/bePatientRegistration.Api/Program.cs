using bePatientRegistration.Application.Patients.Dtos;
using bePatientRegistration.Application.Patients.Services;
using bePatientRegistration.Application.Patients.ServicesImpl;
using bePatientRegistration.Application.Patients.Validators;
using bePatientRegistration.Application.Repositories;
using bePatientRegistration.Infrastructure.Persistence;
using bePatientRegistration.Infrastructure.Persistence.Seed;
using bePatientRegistration.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// CORS para o Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ... resto dos services (AddControllers, AddDbContext, etc)



// 1) Controllers
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePatientRequestValidator>();


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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    ApplicationDbSeeder.Seed(dbContext);
}

// 6) PIPELINE HTTP

// Swagger SEM condicional de ambiente, pra facilitar desenvolvimento
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "bePatientRegistration API v1");
    c.RoutePrefix = "swagger"; // URL: /swagger
});

app.UseHttpsRedirection();

// habilita CORS ANTES do MapControllers
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();

