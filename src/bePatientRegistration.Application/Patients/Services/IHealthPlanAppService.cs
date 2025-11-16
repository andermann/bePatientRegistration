using bePatientRegistration.Application.Patients.Dtos;

namespace bePatientRegistration.Application.Patients.Services
{
    public interface IHealthPlanAppService
    {
        Task<IReadOnlyList<HealthPlanDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<HealthPlanDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<HealthPlanDto> CreateAsync(CreateHealthPlanRequest request, CancellationToken cancellationToken = default);
        Task<HealthPlanDto?> UpdateAsync(Guid id, UpdateHealthPlanRequest request, CancellationToken cancellationToken = default);
    }
}
