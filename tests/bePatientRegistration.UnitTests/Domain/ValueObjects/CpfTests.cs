using bePatientRegistration.Domain.Exceptions;
using bePatientRegistration.Domain.ValueObjects;

namespace bePatientRegistration.UnitTests.Domain.ValueObjects;

public class CpfTests
{
    // CPF válido gerado com o algoritmo padrão de CPF
    // (9 primeiros dígitos "123456789" + dígitos verificadores calculados)
    private const string ValidCpfDigits = "12345678909";

    [Fact]
    public void Constructor_Should_NormalizeAndStoreDigits_WhenCpfIsValid()
    {
        // Arrange
        var raw = "123.456.789-09";

        // Act
        var cpf = new Cpf(raw);

        // Assert
        Assert.Equal(ValidCpfDigits, cpf.Value);
        Assert.Equal(ValidCpfDigits, cpf.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_Should_ThrowDomainException_WhenCpfIsEmpty(string? value)
    {
        // Act
        var ex = Assert.Throws<DomainException>(() => new Cpf(value!));

        // Assert
        Assert.Equal("CPF não pode ser vazio.", ex.Message);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("11111111111")]
    [InlineData("12345678900")] // dígitos verificadores inválidos
    public void Constructor_Should_ThrowDomainException_WhenCpfIsInvalid(string value)
    {
        // Act
        var ex = Assert.Throws<DomainException>(() => new Cpf(value));

        // Assert
        Assert.Equal("CPF inválido.", ex.Message);
    }

    [Fact]
    public void Normalize_ShouldReturnOnlyDigits()
    {
        // Arrange
        var raw = "123.456.789-09";

        // Act
        var normalized = Cpf.Normalize(raw);

        // Assert
        Assert.Equal(ValidCpfDigits, normalized);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_ForValidCpf()
    {
        // Act
        var result = Cpf.IsValid(ValidCpfDigits);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("11111111111")]
    [InlineData("12345678900")]
    public void IsValid_ShouldReturnFalse_ForInvalidCpf(string value)
    {
        // Act
        var result = Cpf.IsValid(value);

        // Assert
        Assert.False(result);
    }
}
