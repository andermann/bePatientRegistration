using bePatientRegistration.Application.Patients.Dtos;
using bePatientRegistration.Application.Patients.Services;
using bePatientRegistration.Application.Repositories;
using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Domain.Exceptions;

namespace bePatientRegistration.Application.Patients.Services
{
    public class HealthPlanAppService : IHealthPlanAppService
    {
        private readonly IHealthPlanRepository _healthPlanRepository;

        public HealthPlanAppService(IHealthPlanRepository healthPlanRepository)
        {
            _healthPlanRepository = healthPlanRepository;
        }

        public async Task<IReadOnlyList<HealthPlanDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var list = await _healthPlanRepository.GetAllAsync(cancellationToken);
            return list.Select(MapToDto).ToList();
        }

        public async Task<HealthPlanDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var healthPlan = await _healthPlanRepository.GetByIdAsync(id, cancellationToken);
            return healthPlan is null ? null : MapToDto(healthPlan);
        }

        public async Task<HealthPlanDto> CreateAsync(CreateHealthPlanRequest request, CancellationToken cancellationToken = default)
        {
            if (await _healthPlanRepository.ExistsByNameAsync(request.Name, cancellationToken))
                throw new DomainException("Já existe um convênio com esse nome.");

            var healthPlan = new HealthPlan(request.Name);

            await _healthPlanRepository.AddAsync(healthPlan, cancellationToken);
            await _healthPlanRepository.SaveChangesAsync(cancellationToken);

            return MapToDto(healthPlan);
        }

        public async Task<HealthPlanDto?> UpdateAsync(Guid id, UpdateHealthPlanRequest request, CancellationToken cancellationToken = default)
        {
            var existing = await _healthPlanRepository.GetByIdAsync(id, cancellationToken);
            if (existing is null)
                return null;

            // não fizemos método específico no domínio, mas você pode adicionar se quiser
            if (!string.Equals(existing.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (await _healthPlanRepository.ExistsByNameAsync(request.Name, cancellationToken))
                    throw new DomainException("Já existe um convênio com esse nome.");
            }

            // atualização simples
            typeof(HealthPlan)
                .GetProperty(nameof(HealthPlan.Name))!
                .SetValue(existing, request.Name);

            typeof(HealthPlan)
                .GetProperty(nameof(HealthPlan.IsActive))!
                .SetValue(existing, request.IsActive);

            await _healthPlanRepository.UpdateAsync(existing, cancellationToken);
            await _healthPlanRepository.SaveChangesAsync(cancellationToken);

            return MapToDto(existing);
        }

        private static HealthPlanDto MapToDto(HealthPlan healthPlan)
        {
            return new HealthPlanDto
            {
                Id = healthPlan.Id,
                Name = healthPlan.Name,
                IsActive = healthPlan.IsActive
            };
        }
    }
}
