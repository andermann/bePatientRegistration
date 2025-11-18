using bePatientRegistration.Domain.Entities;

namespace bePatientRegistration.UnitTests.Domain.Entities;

public class HealthPlanTests
{
    [Fact]
    public void Constructor_Should_SetPropertiesAndActivatePlan()
    {
        // Arrange
        var name = "Plano Ouro";

        // Act
        var plan = new HealthPlan(name);

        // Assert
        Assert.NotEqual(Guid.Empty, plan.Id);
        Assert.Equal(name, plan.Name);
        Assert.True(plan.IsActive);
        Assert.NotNull(plan.Patients);
        Assert.Empty(plan.Patients);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_WhenNameIsNull()
    {
        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => new HealthPlan(null!));

        // Assert
        Assert.Equal("name", ex.ParamName);
    }

    [Fact]
    public void Deactivate_Should_SetIsActiveFalse()
    {
        // Arrange
        var plan = new HealthPlan("Plano Prata");

        // Act
        plan.Deactivate();

        // Assert
        Assert.False(plan.IsActive);
    }
}
