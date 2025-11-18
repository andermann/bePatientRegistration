using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using bePatientRegistration.Application.Patients.Dtos;
using bePatientRegistration.Application.Patients.Services;
using bePatientRegistration.Application.Repositories;
using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Domain.Exceptions;
using Moq;
using Xunit;

namespace bePatientRegistration.UnitTests.Application.HealthPlans.Services
{
    public class HealthPlanAppServiceTests
    {
        private readonly Mock<IHealthPlanRepository> _healthPlanRepositoryMock;
        private readonly HealthPlanAppService _sut;

        public HealthPlanAppServiceTests()
        {
            _healthPlanRepositoryMock = new Mock<IHealthPlanRepository>(MockBehavior.Strict);
            _sut = new HealthPlanAppService(_healthPlanRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            // Arrange
            var plans = new List<HealthPlan>
            {
                new HealthPlan("Plano A"),
                new HealthPlan("Plano B")
            };

            _healthPlanRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(plans);

            // Act
            var result = await _sut.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(plans.Count, result.Count);
            Assert.Contains(result, x => x.Name == "Plano A");
            Assert.Contains(result, x => x.Name == "Plano B");

            _healthPlanRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            _healthPlanRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthPlan?)null);

            // Act
            var result = await _sut.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _healthPlanRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDto_WhenExists()
        {
            // Arrange
            var plan = new HealthPlan("Plano A");

            _healthPlanRepositoryMock
                .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(plan);

            // Act
            var result = await _sut.GetByIdAsync(plan.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(plan.Id, result!.Id);
            Assert.Equal(plan.Name, result.Name);
            Assert.Equal(plan.IsActive, result.IsActive);

            _healthPlanRepositoryMock.Verify(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowDomainException_WhenNameAlreadyExists()
        {
            // Arrange
            var request = new CreateHealthPlanRequest
            {
                Name = "Plano A"
            };

            _healthPlanRepositoryMock
                .Setup(r => r.ExistsByNameAsync(request.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DomainException>(() => _sut.CreateAsync(request, CancellationToken.None));
            Assert.Equal("Já existe um convênio com esse nome.", ex.Message);

            _healthPlanRepositoryMock.Verify(r => r.ExistsByNameAsync(request.Name, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAsync_ShouldPersistAndReturnDto_WhenValid()
        {
            // Arrange
            var request = new CreateHealthPlanRequest
            {
                Name = "Plano Novo"
            };

            _healthPlanRepositoryMock
                .Setup(r => r.ExistsByNameAsync(request.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            HealthPlan? addedPlan = null;

            _healthPlanRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<HealthPlan>(), It.IsAny<CancellationToken>()))
                .Returns<HealthPlan, CancellationToken>((plan, _) =>
                {
                    addedPlan = plan;
                    return Task.CompletedTask;
                });

            _healthPlanRepositoryMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            Assert.True(result.IsActive);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.NotNull(addedPlan);
            Assert.Equal(addedPlan!.Id, result.Id);

            _healthPlanRepositoryMock.Verify(r => r.ExistsByNameAsync(request.Name, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.Verify(r => r.AddAsync(It.IsAny<HealthPlan>(), It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenPlanDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateHealthPlanRequest
            {
                Name = "Novo Nome",
                IsActive = true
            };

            _healthPlanRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthPlan?)null);

            // Act
            var result = await _sut.UpdateAsync(id, request, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _healthPlanRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowDomainException_WhenChangingToExistingName()
        {
            // Arrange
            var existing = new HealthPlan("Plano Atual");
            var id = existing.Id;

            var request = new UpdateHealthPlanRequest
            {
                Name = "Nome Duplicado",
                IsActive = true
            };

            _healthPlanRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            _healthPlanRepositoryMock
                .Setup(r => r.ExistsByNameAsync(request.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DomainException>(() => _sut.UpdateAsync(id, request, CancellationToken.None));
            Assert.Equal("Já existe um convênio com esse nome.", ex.Message);

            _healthPlanRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.Verify(r => r.ExistsByNameAsync(request.Name, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAndReturnDto_WhenValid()
        {
            // Arrange
            var existing = new HealthPlan("Plano Antigo");
            var id = existing.Id;

            var request = new UpdateHealthPlanRequest
            {
                Name = "Plano Atualizado",
                IsActive = false
            };

            _healthPlanRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            _healthPlanRepositoryMock
                .Setup(r => r.ExistsByNameAsync(request.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _healthPlanRepositoryMock
                .Setup(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _healthPlanRepositoryMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.UpdateAsync(id, request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.IsActive, result.IsActive);

            _healthPlanRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.Verify(r => r.ExistsByNameAsync(request.Name, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _healthPlanRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
