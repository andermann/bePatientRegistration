using System;
using bePatientRegistration.Application.Patients.Dtos;
using bePatientRegistration.Application.Patients.Validators;
using bePatientRegistration.Domain.Entities;
using Xunit;

namespace bePatientRegistration.UnitTests.Application.Patients.Validators
{
    public class UpdatePatientRequestValidatorTests
    {
        private readonly UpdatePatientRequestValidator _validator;

        public UpdatePatientRequestValidatorTests()
        {
            _validator = new UpdatePatientRequestValidator();
        }

        private static UpdatePatientRequest CreateValidRequest()
        {
            return new UpdatePatientRequest
            {
                FirstName = "João",
                LastName = "Silva",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                Cpf = "12345678901",
                Rg = "1234567",
                UfRg = Uf.RJ,
                Email = "joao.silva@example.com",
                MobilePhone = "21999999999",
                LandlinePhone = "2133333333",
                HealthPlanId = Guid.NewGuid(),
                HealthPlanCardNumber = "ABC123",
                HealthPlanCardExpirationMonth = 12,
                HealthPlanCardExpirationYear = DateTime.Today.Year + 1,
                IsActive = true
            };
        }

        [Fact]
        public void Should_Be_Valid_When_All_Data_Is_Correct()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Empty()
        {
            // Arrange
            var request = CreateValidRequest();
            request.FirstName = string.Empty;

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e =>
                e.PropertyName == nameof(request.FirstName) &&
                e.ErrorMessage == "Nome é obrigatório.");
        }

        [Fact]
        public void Should_Allow_Toggling_IsActive()
        {
            // Arrange
            var request = CreateValidRequest();
            request.IsActive = false;

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
