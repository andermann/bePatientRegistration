using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Domain.Exceptions;
using bePatientRegistration.Domain.ValueObjects;

namespace bePatientRegistration.UnitTests.Domain.Entities;

public class PatientTests
{
    private static Patient CreateValidPatient(
        string firstName = "João",
        string lastName = "Silva",
        string cpf = "12345678909",
        string rg = "1234567",
        Uf ufRg = Uf.RJ,
        string email = "joao.silva@example.com",
        string mobilePhone = "21999990000",
        string? landlinePhone = null,
        Guid? healthPlanId = null,
        string healthPlanCardNumber = "ABC123456")
    {
        var dob = DateTime.Today.AddYears(-30);
        var planId = healthPlanId ?? Guid.NewGuid();
        var expiration = DateTime.Today.AddMonths(1);

        return new Patient(
            firstName: firstName,
            lastName: lastName,
            dateOfBirth: dob,
            gender: Gender.Male,
            cpf: cpf,
            rg: rg,
            ufRg: ufRg,
            email: email,
            mobilePhone: mobilePhone,
            landlinePhone: landlinePhone,
            healthPlanId: planId,
            healthPlanCardNumber: healthPlanCardNumber,
            healthPlanCardExpirationMonth: expiration.Month,
            healthPlanCardExpirationYear: expiration.Year
        );
    }

    [Fact]
    public void Constructor_Should_CreatePatient_WithValidData()
    {
        // Act
        var patient = CreateValidPatient();

        // Assert
        Assert.NotEqual(Guid.Empty, patient.Id);
        Assert.Equal("João", patient.FirstName);
        Assert.Equal("Silva", patient.LastName);
        Assert.Equal(Gender.Male, patient.Gender);

        Assert.NotNull(patient.Email);
        Assert.Equal("joao.silva@example.com", patient.Email.Value);

        Assert.NotNull(patient.MobilePhone);
        Assert.Equal("21999990000", patient.MobilePhone.Value);
        Assert.Null(patient.LandlinePhone);

        Assert.NotEqual(Guid.Empty, patient.HealthPlanId);
        Assert.Equal("ABC123456", patient.HealthPlanCardNumber);
        Assert.True(patient.IsActive);
        Assert.True(patient.CreatedAt <= DateTime.UtcNow);
        Assert.Null(patient.UpdatedAt);
    }

    [Theory]
    [InlineData(null, "Silva", "Nome é obrigatório.")]
    [InlineData("", "Silva", "Nome é obrigatório.")]
    [InlineData("João", null, "Sobrenome é obrigatório.")]
    [InlineData("João", "", "Sobrenome é obrigatório.")]
    public void SetName_Should_ThrowDomainException_WhenNameIsInvalid(
        string? firstName,
        string? lastName,
        string expectedMessage)
    {
        // Arrange
        var patient = CreateValidPatient();

        // Act
        var ex = Assert.Throws<DomainException>(() => patient.SetName(firstName!, lastName!));

        // Assert
        Assert.Equal(expectedMessage, ex.Message);
    }

    [Fact]
    public void SetName_Should_TrimAndSet_WhenValid()
    {
        // Arrange
        var patient = CreateValidPatient();

        // Act
        patient.SetName("  Maria  ", "  Souza  ");

        // Assert
        Assert.Equal("Maria", patient.FirstName);
        Assert.Equal("Souza", patient.LastName);
    }

    [Fact]
    public void SetDateOfBirth_Should_ThrowDomainException_WhenDateIsFuture()
    {
        // Arrange
        var patient = CreateValidPatient();
        var futureDate = DateTime.Today.AddDays(1);

        // Act
        var ex = Assert.Throws<DomainException>(() => patient.SetDateOfBirth(futureDate));

        // Assert
        Assert.Equal("Data de nascimento não pode ser futura.", ex.Message);
    }

    [Fact]
    public void SetDateOfBirth_Should_SetDate_WhenValid()
    {
        // Arrange
        var patient = CreateValidPatient();
        var date = new DateTime(1990, 5, 10);

        // Act
        patient.SetDateOfBirth(date);

        // Assert
        Assert.Equal(date.Date, patient.DateOfBirth);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void SetCardExpiration_Should_ThrowDomainException_WhenMonthIsInvalid(int month)
    {
        // Arrange
        var patient = CreateValidPatient();

        // Act
        var ex = Assert.Throws<DomainException>(() =>
            patient.SetCardExpiration(month, DateTime.Today.Year));

        // Assert
        Assert.Equal("Mês de validade do convênio inválido.", ex.Message);
    }

    [Fact]
    public void SetCardExpiration_Should_ThrowDomainException_WhenDateIsInThePast()
    {
        // Arrange
        var patient = CreateValidPatient();
        var past = DateTime.Today.AddMonths(-1);

        // Act
        var ex = Assert.Throws<DomainException>(() =>
            patient.SetCardExpiration(past.Month, past.Year));

        // Assert
        Assert.Equal("Carteirinha do convênio está vencida.", ex.Message);
    }

    [Fact]
    public void SetCardExpiration_Should_Set_WhenDateIsValid()
    {
        // Arrange
        var patient = CreateValidPatient();
        var future = DateTime.Today.AddMonths(2);

        // Act
        patient.SetCardExpiration(future.Month, future.Year);

        // Assert
        Assert.Equal(future.Month, patient.HealthPlanCardExpirationMonth);
        Assert.Equal(future.Year, patient.HealthPlanCardExpirationYear);
    }

    [Fact]
    public void Constructor_Should_ThrowDomainException_WhenRgIsEmpty()
    {
        // Act
        var ex = Assert.Throws<DomainException>(() =>
            CreateValidPatient(rg: " "));

        // Assert
        Assert.Equal("RG é obrigatório.", ex.Message);
    }

    [Fact]
    public void Constructor_Should_ThrowDomainException_WhenHealthPlanCardNumberIsEmpty()
    {
        // Act
        var ex = Assert.Throws<DomainException>(() =>
            CreateValidPatient(healthPlanCardNumber: " "));

        // Assert
        Assert.Equal("Número da carteirinha é obrigatório.", ex.Message);
    }

    [Fact]
    public void ValidatePhones_Should_ThrowDomainException_WhenBothPhonesAreNullOrEmpty()
    {
        // Act
        var ex = Assert.Throws<DomainException>(() =>
            Patient.ValidatePhones(null, null));

        // Assert
        Assert.Equal("Informe pelo menos um telefone (celular ou fixo).", ex.Message);
    }

    [Fact]
    public void Update_Should_ChangeProperties_AndSetUpdatedAtAndIsActive()
    {
        // Arrange
        var patient = CreateValidPatient();
        var newDob = DateTime.Today.AddYears(-25);
        var future = DateTime.Today.AddMonths(3);
        var healthPlanId = Guid.NewGuid();

        // Act
        patient.Update(
            firstName: "Ana",
            lastName: "Costa",
            dateOfBirth: newDob,
            gender: Gender.Female,
            cpf: "12345678909",
            rg: "7654321",
            ufRg: Uf.SP,
            email: "ana.costa@example.com",
            mobilePhone: "11988887777",
            landlinePhone: "1133334444",
            healthPlanId: healthPlanId,
            healthPlanCardNumber: "XYZ987654",
            healthPlanCardExpirationMonth: future.Month,
            healthPlanCardExpirationYear: future.Year,
            isActive: false
        );

        // Assert
        Assert.Equal("Ana", patient.FirstName);
        Assert.Equal("Costa", patient.LastName);
        Assert.Equal(newDob.Date, patient.DateOfBirth);
        Assert.Equal(Gender.Female, patient.Gender);

        Assert.NotNull(patient.Cpf);
        Assert.Equal("12345678909", patient.Cpf!.Value);

        Assert.Equal("7654321", patient.Rg);
        Assert.Equal(Uf.SP, patient.UfRg);

        Assert.Equal("ana.costa@example.com", patient.Email.Value);
        Assert.Equal("11988887777", patient.MobilePhone.Value);
        Assert.NotNull(patient.LandlinePhone);
        Assert.Equal("1133334444", patient.LandlinePhone!.Value);

        Assert.Equal(healthPlanId, patient.HealthPlanId);
        Assert.Equal("XYZ987654", patient.HealthPlanCardNumber);
        Assert.Equal(future.Month, patient.HealthPlanCardExpirationMonth);
        Assert.Equal(future.Year, patient.HealthPlanCardExpirationYear);

        Assert.False(patient.IsActive);
        Assert.NotNull(patient.UpdatedAt);
        Assert.True(patient.UpdatedAt!.Value <= DateTime.UtcNow);
    }

    [Fact]
    public void Deactivate_Should_SetIsActiveFalse_AndUpdatedAt()
    {
        // Arrange
        var patient = CreateValidPatient();

        // Act
        patient.Deactivate();

        // Assert
        Assert.False(patient.IsActive);
        Assert.NotNull(patient.UpdatedAt);
        Assert.True(patient.UpdatedAt!.Value <= DateTime.UtcNow);
    }
}
