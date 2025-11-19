using bePatientRegistration.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace bePatientRegistration.IntegrationTests
{
    public class ApiFactory : WebApplicationFactory<bePatientRegistration.Api.Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Força o ambiente de testes de integração
            builder.UseEnvironment("IntegrationTests");

            builder.ConfigureServices(services =>
            {
                // Aqui NÃO existe AddDbContext da API,
                // porque o Program.cs não registra DbContext em IntegrationTests

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("bePatientRegistration_TestDB");
                });

                // Cria escopo e faz seed
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureCreated();
                IntegrationTestDataSeeder.Seed(db);
            });
        }
    }
}
