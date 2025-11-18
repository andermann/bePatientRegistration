using bePatientRegistration.Domain.Exceptions;
using bePatientRegistration.Domain.ValueObjects;

namespace bePatientRegistration.UnitTests.Domain.ValueObjects;

public class PhoneNumberTests
{
    [Fact]
    public void Constructor_Should_NormalizeAndStoreDigits_WhenPhoneIsValid()
    {
        // Arrange
        var raw = "(21) 99999-0000";

        // Act
        var phone = new PhoneNumber(raw);

        // Assert
        Assert.Equal("21999990000", phone.Value);
        Assert.Equal("21999990000", phone.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_Should_ThrowDomainException_WhenPhoneIsEmpty(string? value)
    {
        // Act
        var ex = Assert.Throws<DomainException>(() => new PhoneNumber(value!));

        // Assert
        Assert.Equal("Telefone não pode ser vazio.", ex.Message);
    }

    [Theory]
    [InlineData("123456789")]         // 9 dígitos (curto demais)
    [InlineData("123456789012")]      // 12 dígitos (longo demais)
    public void Constructor_Should_ThrowDomainException_WhenPhoneLengthIsInvalid(string value)
    {
        // Act
        var ex = Assert.Throws<DomainException>(() => new PhoneNumber(value));

        // Assert
        Assert.Equal("Telefone inválido.", ex.Message);
    }

    [Fact]
    public void Normalize_ShouldReturnOnlyDigits()
    {
        // Arrange
        var raw = "+55 (21) 99999-0000";

        // Act
        var normalized = PhoneNumber.Normalize(raw);

        // Assert
        Assert.Equal("5521999990000", normalized);
    }
}
