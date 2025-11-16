//using bePatientRegistration.Infrastructure.Persistence;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers();

//// Swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

////// 🔥 AQUI FICA A CONFIGURAÇÃO DO EF CORE + SQL SERVER 🔥
////var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

////builder.Services.AddDbContext<ApplicationDbContext>(options =>
////    options.UseSqlServer(connectionString, sql =>
////        sql.EnableRetryOnFailure(
////            maxRetryCount: 5,
////            maxRetryDelay: TimeSpan.FromSeconds(10),
////            errorNumbersToAdd: null)));

//var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

//if (useInMemory)
//{
//    //    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    //        options.UseInMemoryDatabase("BePatientRegistrationDb"));
//}
//else
//{
//    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//        options.UseSqlServer(connectionString));
//}


//// TODO: registrar serviços da camada Application e Infra
//// builder.Services.AddScoped<IPatientAppService, PatientAppService>();
//// builder.Services.AddScoped<IPatientRepository, PatientRepository>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.MapControllers();

//app.Run();


using bePatientRegistration.Application.Patients.Services;
using bePatientRegistration.Application.Patients.ServicesImpl;
using bePatientRegistration.Application.Repositories;
using bePatientRegistration.Infrastructure.Persistence;
using bePatientRegistration.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (mantém como está hoje)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🔥 Application Services
builder.Services.AddScoped<IPatientAppService, PatientAppService>();
builder.Services.AddScoped<IHealthPlanAppService, HealthPlanAppService>();

// 🔥 Repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IHealthPlanRepository, HealthPlanRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

