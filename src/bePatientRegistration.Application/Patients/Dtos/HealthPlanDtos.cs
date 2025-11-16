using System.ComponentModel.DataAnnotations;

namespace bePatientRegistration.Application.Patients.Dtos
{
    public class HealthPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; }
    }

    public class CreateHealthPlanRequest
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = default!;
    }

    public class UpdateHealthPlanRequest
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; } = true;
    }
}
