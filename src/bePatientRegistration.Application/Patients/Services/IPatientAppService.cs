using bePatientRegistration.Application.Patients.Dtos;

namespace bePatientRegistration.Application.Patients.Services
{
    public interface IPatientAppService
    {
        Task<PatientDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<PatientDto> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken = default);
        Task<PatientDto?> UpdateAsync(Guid id, UpdatePatientRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
