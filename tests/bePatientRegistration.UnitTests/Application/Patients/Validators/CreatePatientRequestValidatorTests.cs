using System;
using bePatientRegistration.Application.Patients.Dtos;
using bePatientRegistration.Application.Patients.Validators;
using bePatientRegistration.Domain.Entities;
using Xunit;

namespace bePatientRegistration.UnitTests.Application.Patients.Validators
{
    public class CreatePatientRequestValidatorTests
    {
        private readonly CreatePatientRequestValidator _validator;

        public CreatePatientRequestValidatorTests()
        {
            _validator = new CreatePatientRequestValidator();
        }

        private static CreatePatientRequest CreateValidRequest()
        {
            return new CreatePatientRequest
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
        public void Should_Have_Error_When_Cpf_Is_Invalid()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Cpf = "123"; // menos de 11 dígitos numéricos

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e =>
                e.PropertyName == nameof(request.Cpf) &&
                e.ErrorMessage == "CPF deve ter 11 dígitos numéricos.");
        }

        [Fact]
        public void Should_Have_Error_When_No_Phone_Informed()
        {
            // Arrange
            var request = CreateValidRequest();
            request.MobilePhone = string.Empty;
            request.LandlinePhone = string.Empty;

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e =>
                e.ErrorMessage == "Informe pelo menos um telefone (celular ou fixo).");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Email = "email_invalido";

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e =>
                e.PropertyName == nameof(request.Email) &&
                e.ErrorMessage == "E-mail inválido.");
        }

        [Fact]
        public void Should_Have_Error_When_HealthCard_ExpirationMonth_Is_Invalid()
        {
            // Arrange
            var request = CreateValidRequest();
            request.HealthPlanCardExpirationMonth = 13;

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e =>
                e.PropertyName == nameof(request.HealthPlanCardExpirationMonth) &&
                e.ErrorMessage == "Mês de validade deve estar entre 1 e 12.");
        }

        [Fact]
        public void Should_Have_Error_When_HealthCard_ExpirationYear_Is_Less_Than_Current()
        {
            // Arrange
            var request = CreateValidRequest();
            request.HealthPlanCardExpirationYear = DateTime.Today.Year - 1;

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e =>
                e.PropertyName == nameof(request.HealthPlanCardExpirationYear) &&
                e.ErrorMessage == "Ano de validade não pode ser menor que o ano atual.");
        }
    }
}
