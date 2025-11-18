using System.Collections.Generic;
using System.Linq;
using bePatientRegistration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bePatientRegistration.Infrastructure.Persistence.Seed
{
    public static class ApplicationDbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Garante que o banco está criado e com as migrações aplicadas
            context.Database.Migrate();

            // Se já existirem convênios cadastrados, não faz nada
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
