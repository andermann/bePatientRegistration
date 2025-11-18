using bePatientRegistration.Application.Patients.Dtos;
using bePatientRegistration.Application.Patients.Services;
using bePatientRegistration.Application.Repositories;
using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Domain.Exceptions;
using bePatientRegistration.Domain.ValueObjects;

namespace bePatientRegistration.Application.Patients.ServicesImpl
{
    public class PatientAppService : IPatientAppService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IHealthPlanRepository _healthPlanRepository;

        public PatientAppService(
            IPatientRepository patientRepository,
            IHealthPlanRepository healthPlanRepository)
        {
            _patientRepository = patientRepository;
            _healthPlanRepository = healthPlanRepository;
        }

        public async Task<PatientDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var patient = await _patientRepository.GetByIdAsync(id, cancellationToken);
            return patient is null ? null : MapToDto(patient);
        }

        public async Task<IReadOnlyList<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var patients = await _patientRepository.GetAllAsync(cancellationToken);
            return patients.Select(MapToDto).ToList();
        }

        public async Task<PatientDto> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken = default)
        {
            // valida se convênio existe
            var healthPlan = await _healthPlanRepository.GetByIdAsync(request.HealthPlanId, cancellationToken);
            if (healthPlan is null)
                throw new DomainException("Convênio informado não existe.");

            // regra simples: não permitir CPF duplicado
            if (!string.IsNullOrWhiteSpace(request.Cpf))
            {
                var normalizedCpf = Cpf.Normalize(request.Cpf);
                bool existsCpf = await _patientRepository.ExistsByCpfAsync(normalizedCpf, cancellationToken);
                if (existsCpf)
                    throw new DomainException("Já existe um paciente cadastrado com este CPF.");
            }

            var patient = new Patient(
                firstName: request.FirstName,
                lastName: request.LastName,
                dateOfBirth: request.DateOfBirth,
                gender: request.Gender,
                cpf: request.Cpf,
                rg: request.Rg,
                ufRg: request.UfRg,
                email: request.Email,
                mobilePhone: request.MobilePhone,
                landlinePhone: request.LandlinePhone,
                healthPlanId: request.HealthPlanId,
                healthPlanCardNumber: request.HealthPlanCardNumber,
                healthPlanCardExpirationMonth: request.HealthPlanCardExpirationMonth,
                healthPlanCardExpirationYear: request.HealthPlanCardExpirationYear
            );

            await _patientRepository.AddAsync(patient, cancellationToken);
            await _patientRepository.SaveChangesAsync(cancellationToken);

            // carregar convênio para preencher o DTO
            patient = await _patientRepository.GetByIdAsync(patient.Id, cancellationToken)
                      ?? patient;

            return MapToDto(patient);
        }

        public async Task<PatientDto?> UpdateAsync(Guid id, UpdatePatientRequest request, CancellationToken cancellationToken = default)
        {
            var existing = await _patientRepository.GetByIdAsync(id, cancellationToken);
            if (existing is null)
                return null;

            var healthPlan = await _healthPlanRepository.GetByIdAsync(request.HealthPlanId, cancellationToken);
            if (healthPlan is null)
                throw new DomainException("Convênio informado não existe.");

            existing.Update(
                firstName: request.FirstName,
                lastName: request.LastName,
                dateOfBirth: request.DateOfBirth,
                gender: request.Gender,
                cpf: request.Cpf,
                rg: request.Rg,
                ufRg: request.UfRg,
                email: request.Email,
                mobilePhone: request.MobilePhone,
                landlinePhone: request.LandlinePhone,
                healthPlanId: request.HealthPlanId,
                healthPlanCardNumber: request.HealthPlanCardNumber,
                healthPlanCardExpirationMonth: request.HealthPlanCardExpirationMonth,
                healthPlanCardExpirationYear: request.HealthPlanCardExpirationYear, 
                isActive: request.IsActive
            );

            if (!request.IsActive && existing.IsActive)
            {
                existing.Deactivate();
            }

            await _patientRepository.UpdateAsync(existing, cancellationToken);
            await _patientRepository.SaveChangesAsync(cancellationToken);

            var updated = await _patientRepository.GetByIdAsync(id, cancellationToken) ?? existing;
            return MapToDto(updated);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _patientRepository.GetByIdAsync(id, cancellationToken);
            if (existing is null)
                return;

            await _patientRepository.DeleteAsync(existing, cancellationToken);
            await _patientRepository.SaveChangesAsync(cancellationToken);
        }

        private static PatientDto MapToDto(Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Cpf = patient.Cpf?.Value,
                Rg = patient.Rg,
                UfRg = patient.UfRg,
                Email = patient.Email.Value,
                MobilePhone = patient.MobilePhone.Value,
                LandlinePhone = patient.LandlinePhone?.Value,
                HealthPlanId = patient.HealthPlanId,
                HealthPlanName = patient.HealthPlan?.Name ?? string.Empty,
                HealthPlanCardNumber = patient.HealthPlanCardNumber,
                HealthPlanCardExpirationMonth = patient.HealthPlanCardExpirationMonth,
                HealthPlanCardExpirationYear = patient.HealthPlanCardExpirationYear,
                IsActive = patient.IsActive,
                CreatedAt = patient.CreatedAt
            };
        }
    }
}
