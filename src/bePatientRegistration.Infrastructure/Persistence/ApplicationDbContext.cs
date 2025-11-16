using bePatientRegistration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bePatientRegistration.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<HealthPlan> HealthPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Carrega todas as configurações de IEntityTypeConfiguration<> automaticamente
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
