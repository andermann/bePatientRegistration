using bePatientRegistration.Domain.Exceptions;
using bePatientRegistration.Domain.ValueObjects;

namespace bePatientRegistration.UnitTests.Domain.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Constructor_Should_SetValue_WhenEmailIsValid()
    {
        // Arrange
        var rawEmail = "user@example.com";

        // Act
        var email = new Email(rawEmail);

        // Assert
        Assert.Equal("user@example.com", email.Value);
        Assert.Equal("user@example.com", email.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_Should_ThrowDomainException_WhenEmailIsEmpty(string? value)
    {
        // Act
        var ex = Assert.Throws<DomainException>(() => new Email(value!));

        // Assert
        Assert.Equal("E-mail não pode ser vazio.", ex.Message);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("user@")]
    [InlineData("@example.com")]
    [InlineData("user@example")]
    public void Constructor_Should_ThrowDomainException_WhenEmailIsInvalid(string value)
    {
        // Act
        var ex = Assert.Throws<DomainException>(() => new Email(value));

        // Assert
        Assert.Equal("E-mail inválido.", ex.Message);
    }

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("USER@EXAMPLE.COM")]
    [InlineData("first.last@sub.domain.com")]
    public void IsValid_ShouldReturnTrue_ForValidEmails(string value)
    {
        // Act
        var result = Email.IsValid(value);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData("user@")]
    [InlineData("user@example")]
    [InlineData("user@ example.com")]
    public void IsValid_ShouldReturnFalse_ForInvalidEmails(string value)
    {
        // Act
        var result = Email.IsValid(value);

        // Assert
        Assert.False(result);
    }
}
