using System.ComponentModel.DataAnnotations;
using bePatientRegistration.Domain.Entities;

namespace bePatientRegistration.Application.Patients.Dtos
{
    public class PatientDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string FullName => $"{FirstName} {LastName}";

        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public string? Cpf { get; set; }
        public string Rg { get; set; } = default!;
        public Uf UfRg { get; set; }

        public string Email { get; set; } = default!;
        public string MobilePhone { get; set; } = default!;
        public string? LandlinePhone { get; set; }

        public Guid HealthPlanId { get; set; }
        public string HealthPlanName { get; set; } = default!;
        public string HealthPlanCardNumber { get; set; } = default!;
        public int HealthPlanCardExpirationMonth { get; set; }
        public int HealthPlanCardExpirationYear { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreatePatientRequest
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = default!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = default!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        // opcional, mas se vier deve ser válido
        public string? Cpf { get; set; }

        [Required, MaxLength(20)]
        public string Rg { get; set; } = default!;

        [Required]
        public Uf UfRg { get; set; }

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = default!;

        [Required, MaxLength(20)]
        public string MobilePhone { get; set; } = default!;

        [MaxLength(20)]
        public string? LandlinePhone { get; set; }

        [Required]
        public Guid HealthPlanId { get; set; }

        [Required, MaxLength(50)]
        public string HealthPlanCardNumber { get; set; } = default!;

        [Range(1, 12)]
        public int HealthPlanCardExpirationMonth { get; set; }

        [Range(2024, 2100)]
        public int HealthPlanCardExpirationYear { get; set; }
    }

    public class UpdatePatientRequest : CreatePatientRequest
    {
        public bool IsActive { get; set; } = true;
    }
}
