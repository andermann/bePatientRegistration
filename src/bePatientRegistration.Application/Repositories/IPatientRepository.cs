using bePatientRegistration.Domain.Entities;

namespace bePatientRegistration.Application.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Patient>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(Patient patient, CancellationToken cancellationToken = default);
        Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
        Task DeleteAsync(Patient patient, CancellationToken cancellationToken = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
