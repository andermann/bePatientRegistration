using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace bePatientRegistration.Infrastructure.Persistence.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Pacientes");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.DateOfBirth)
                .IsRequired();

            builder.Property(p => p.Gender)
                .IsRequired();

            builder.Property(p => p.Rg)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.UfRg)
                .IsRequired();

            // Owned types: Cpf
            builder.OwnsOne(p => p.Cpf, cpf =>
            {
                cpf.Property(c => c.Value)
                    .HasColumnName("Cpf")
                    .HasMaxLength(11);
            });

            // Email
            builder.OwnsOne(p => p.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(200);
            });

            // MobilePhone
            builder.OwnsOne(p => p.MobilePhone, phone =>
            {
                phone.Property(ph => ph.Value)
                    .HasColumnName("MobilePhone")
                    .IsRequired()
                    .HasMaxLength(20);
            });

            // LandlinePhone (opcional)
            builder.OwnsOne(p => p.LandlinePhone, phone =>
            {
                phone.Property(ph => ph.Value)
                    .HasColumnName("LandlinePhone")
                    .HasMaxLength(20);
            });

            builder.Property(p => p.HealthPlanCardNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.HealthPlanCardExpirationMonth)
                .IsRequired();

            builder.Property(p => p.HealthPlanCardExpirationYear)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .IsRequired(false);

            // Relacionamento com HealthPlan
            builder.HasOne(p => p.HealthPlan)
                .WithMany(h => h.Patients)
                .HasForeignKey(p => p.HealthPlanId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
