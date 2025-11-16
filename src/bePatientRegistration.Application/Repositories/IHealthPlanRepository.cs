using bePatientRegistration.Domain.Entities;

namespace bePatientRegistration.Application.Repositories
{
    public interface IHealthPlanRepository
    {
        Task<HealthPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<HealthPlan>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(HealthPlan healthPlan, CancellationToken cancellationToken = default);
        Task UpdateAsync(HealthPlan healthPlan, CancellationToken cancellationToken = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
