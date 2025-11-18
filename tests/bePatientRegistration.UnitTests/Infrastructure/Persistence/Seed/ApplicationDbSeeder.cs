using bePatientRegistration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bePatientRegistration.Infrastructure.Persistence.Seed
{
    public static class ApplicationDbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Garante que o banco e o schema existem para o provider atual
            // (InMemory, SQL Server, SQLite, etc.) SEM usar migrations.
            context.Database.EnsureCreated();

            // Se já houver convênios, não faz nada (idempotente)
            if (context.HealthPlans.Any())
                return;

            var defaultHealthPlans = new List<HealthPlan>
            {
                new HealthPlan("Amil Saúde"),
                new HealthPlan("Bradesco Saúde"),
                new HealthPlan("Unimed Nacional"),
                new HealthPlan("SulAmérica Saúde"),
                new HealthPlan("Golden Cross")
            };

            context.HealthPlans.AddRange(defaultHealthPlans);
            context.SaveChanges();
        }
    }
}
