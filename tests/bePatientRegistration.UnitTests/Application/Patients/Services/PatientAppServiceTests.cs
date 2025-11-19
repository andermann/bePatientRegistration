using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using bePatientRegistration.Application.Patients.Dtos;
using bePatientRegistration.Application.Patients.Services;
using bePatientRegistration.Application.Patients.ServicesImpl;
using bePatientRegistration.Application.Repositories;
using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Domain.Exceptions;
using bePatientRegistration.Domain.ValueObjects;
using Moq;
using Xunit;

namespace bePatientRegistration.UnitTests.Application.Patients.Services
{
    public class PatientAppServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IHealthPlanRepository> _healthPlanRepositoryMock;
        private readonly PatientAppService _sut;

        public PatientAppServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>(MockBehavior.Strict);
            _healthPlanRepositoryMock = new Mock<IHealthPlanRepository>(MockBehavior.Strict);

            _sut = new PatientAppService(
                _patientRepositoryMock.Object,
                _healthPlanRepositoryMock.Object);
        }

        #region Helpers

        private static Patient CreateValidPatient(Guid? id = null, Guid? healthPlanId = null, string cpf = "12345678909")
        {
            var planId = healthPlanId ?? Guid.NewGuid();

            var patient = new Patient(
                firstName: "João",
                lastName: "Silva",
                dateOfBirth: new DateTime(1990, 1, 1),
                gender: Gender.Male,
                cpf: cpf,
                rg: "1234567",
                ufRg: Uf.RJ,
                email: "joao.silva@example.com",
                mobilePhone: "21999999999",
                landlinePhone: null,
                healthPlanId: planId,
                healthPlanCardNumber: "ABC123",
                healthPlanCardExpirationMonth: 12,
                healthPlanCardExpirationYear: DateTime.Today.Year + 1
            );

            if (id.HasValue)
            {
                typeof(Patient)
                    .GetProperty(nameof(Patient.Id))!
                    .SetValue(patient, id.Value);
            }

            return patient;
        }

        private static HealthPlan CreateValidHealthPlan(Guid? id = null, string name = "Plano Ouro")
        {
            var plan = new HealthPlan(name);

            if (id.HasValue)
            {
                typeof(HealthPlan)
                    .GetProperty(nameof(HealthPlan.Id))!
                    .SetValue(plan, id.Value);
            }

            return plan;
        }

        private static CreatePatientRequest CreateValidRequest(Guid? healthPlanId = null, string cpf = "12345678909")
        {
            return new CreatePatientRequest
            {
                FirstName = "João",
                LastName = "Silva",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                Cpf = cpf,
                Rg = "1234567",
                UfRg = Uf.RJ,
                Email = "joao.silva@example.com",
                MobilePhone = "21999999999",
                LandlinePhone = null,
                HealthPlanId = healthPlanId ?? Guid.NewGuid(),
                HealthPlanCardNumber = "ABC123",
                HealthPlanCardExpirationMonth = 12,
                HealthPlanCardExpirationYear = DateTime.Today.Year + 1,
                IsActive = true
            };
        }

        private static UpdatePatientRequest CreateValidUpdateRequest(Guid? healthPlanId = null, string cpf = "12345678909")
        {
            return new UpdatePatientRequest
            {
                FirstName = "João Atualizado",
                LastName = "Silva Atualizado",
                DateOfBirth = new DateTime(1991, 2, 2),
                Gender = Gender.Female,
                Cpf = cpf,
                Rg = "7654321",
                UfRg = Uf.SP,
                Email = "joao.atualizado@example.com",
                MobilePhone = "21988888888",
                LandlinePhone = "2133333333",
                HealthPlanId = healthPlanId ?? Guid.NewGuid(),
                HealthPlanCardNumber = "XYZ999",
                HealthPlanCardExpirationMonth = 11,
                HealthPlanCardExpirationYear = DateTime.Today.Year + 2,
                IsActive = false
            };
        }

        #endregion

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenPatientDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            // Act
            var result = await _sut.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _patientRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenPatientExists()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var plan = CreateValidHealthPlan(planId, "Plano VIP");
            var patient = CreateValidPatient(healthPlanId: planId);

            typeof(Patient)
                .GetProperty(nameof(Patient.HealthPlan))!
                .SetValue(patient, plan);

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(patient.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            // Act
            var result = await _sut.GetByIdAsync(patient.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patient.Id, result!.Id);
            Assert.Equal(patient.FirstName, result.FirstName);
            Assert.Equal(patient.LastName, result.LastName);
            Assert.Equal(planId, result.HealthPlanId);
            Assert.Equal(plan.Name, result.HealthPlanName);

            _patientRepositoryMock.Verify(r => r.GetByIdAsync(patient.Id, It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var plan = CreateValidHealthPlan(planId, "Plano VIP");

            var patient1 = CreateValidPatient(healthPlanId: planId);
            var patient2 = CreateValidPatient(healthPlanId: planId, cpf: "98765432100");

            typeof(Patient).GetProperty(nameof(Patient.HealthPlan))!.SetValue(patient1, plan);
            typeof(Patient).GetProperty(nameof(Patient.HealthPlan))!.SetValue(patient2, plan);

            var patients = new List<Patient> { patient1, patient2 };

            _patientRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(patients);

            // Act
            var result = await _sut.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, dto => Assert.Equal(planId, dto.HealthPlanId));

            _patientRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowDomainException_WhenHealthPlanDoesNotExist()
        {
            // Arrange
            var request = CreateValidRequest();

            _healthPlanRepositoryMock
                .Setup(r => r.GetByIdAsync(request.HealthPlanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthPlan?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DomainException>(() => _sut.CreateAsync(request, CancellationToken.None));
            Assert.Equal("Convênio informado não existe.", ex.Message);

            _healthPlanRepositoryMock.Verify(r => r.GetByIdAsync(request.HealthPlanId, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
            _patientRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowDomainException_WhenCpfAlreadyExists()
        {
            // Arrange
            var plan = CreateValidHealthPlan();
            var request = CreateValidRequest(healthPlanId: plan.Id, cpf: "12345678909");

            _healthPlanRepositoryMock
                .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(plan);

            _patientRepositoryMock
                .Setup(r => r.ExistsByCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DomainException>(() => _sut.CreateAsync(request, CancellationToken.None));
            Assert.Equal("Já existe um paciente cadastrado com este CPF.", ex.Message);

            _healthPlanRepositoryMock.Verify(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.Verify(r => r.ExistsByCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAsync_ShouldPersistAndReturnDto_WhenValid()
        {
            // Arrange
            var plan = CreateValidHealthPlan();
            var request = CreateValidRequest(healthPlanId: plan.Id, cpf: "12345678909");

            _healthPlanRepositoryMock
                .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(plan);

            _patientRepositoryMock
                .Setup(r => r.ExistsByCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            Patient? addedPatient = null;

            _patientRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .Returns<Patient, CancellationToken>((patient, _) =>
                {
                    addedPatient = patient;
                    return Task.CompletedTask;
                });

            _patientRepositoryMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            //_patientRepositoryMock
            //    .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync((CancellationToken ct) =>
            //    {
            //        return addedPatient!;
            //    });

            _patientRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(addedPatient);


            // Act
            var result = await _sut.CreateAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            //Assert.Equal(plan.Id, result.HealthPlanId);
            //Assert.Equal(plan.Name, result.HealthPlanName);

            _healthPlanRepositoryMock.Verify(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.Verify(r => r.ExistsByCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenPatientDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = CreateValidUpdateRequest();

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            // Act
            var result = await _sut.UpdateAsync(id, request, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _patientRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowDomainException_WhenHealthPlanDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existing = CreateValidPatient(id);
            var request = CreateValidUpdateRequest(healthPlanId: Guid.NewGuid());

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            _healthPlanRepositoryMock
                .Setup(r => r.GetByIdAsync(request.HealthPlanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthPlan?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DomainException>(() => _sut.UpdateAsync(id, request, CancellationToken.None));
            Assert.Equal("Convênio informado não existe.", ex.Message);

            _patientRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.Verify(r => r.GetByIdAsync(request.HealthPlanId, It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAndReturnDto_WhenValid()
        {
            // Arrange
            var plan = CreateValidHealthPlan();
            var id = Guid.NewGuid();
            var existing = CreateValidPatient(id, healthPlanId: plan.Id);
            var request = CreateValidUpdateRequest(healthPlanId: plan.Id, cpf: existing.Cpf?.Value ?? "12345678909");

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            _healthPlanRepositoryMock
                .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(plan);

            _patientRepositoryMock
                .Setup(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            // Act
            var result = await _sut.UpdateAsync(id, request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(plan.Id, result.HealthPlanId);
            //Assert.Equal(plan.Name, result.HealthPlanName);

            _patientRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Exactly(2));
            _healthPlanRepositoryMock.Verify(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
