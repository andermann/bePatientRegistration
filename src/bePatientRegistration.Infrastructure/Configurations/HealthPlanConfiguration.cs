using bePatientRegistration.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace bePatientRegistration.Infrastructure.Persistence.Configurations
{
    public class HealthPlanConfiguration : IEntityTypeConfiguration<HealthPlan>
    {
        public void Configure(EntityTypeBuilder<HealthPlan> builder)
        {
            builder.ToTable("Convenios");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(h => h.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
