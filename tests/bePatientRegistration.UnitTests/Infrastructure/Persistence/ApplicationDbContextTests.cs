using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace bePatientRegistration.UnitTests.Infrastructure.Persistence
{
    public class ApplicationDbContextTests
    {
        private static ApplicationDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public void OnModelCreating_ShouldConfigure_Patient_And_HealthPlan_Entities()
        {
            // Arrange
            using var context = CreateInMemoryContext(
                nameof(OnModelCreating_ShouldConfigure_Patient_And_HealthPlan_Entities));

            // Act
            var model = context.Model;

            var patientEntity = model.FindEntityType(typeof(Patient));
            var healthPlanEntity = model.FindEntityType(typeof(HealthPlan));

            // Assert
            Assert.NotNull(patientEntity);
            Assert.NotNull(healthPlanEntity);

            // Pacientes
            Assert.Equal("Pacientes", patientEntity!.GetTableName());

            var firstNameProp = patientEntity.FindProperty(nameof(Patient.FirstName));
            Assert.NotNull(firstNameProp);
            Assert.False(firstNameProp!.IsNullable);
            Assert.Equal(100, firstNameProp.GetMaxLength());

            var patientIsActiveProp = patientEntity.FindProperty(nameof(Patient.IsActive));
            Assert.NotNull(patientIsActiveProp);
            Assert.True((bool?)patientIsActiveProp!.GetDefaultValue() ?? false);

            // Convênios
            Assert.Equal("Convenios", healthPlanEntity!.GetTableName());

            var planNameProp = healthPlanEntity.FindProperty(nameof(HealthPlan.Name));
            Assert.NotNull(planNameProp);
            Assert.False(planNameProp!.IsNullable);
            Assert.Equal(150, planNameProp.GetMaxLength());

            var planIsActiveProp = healthPlanEntity.FindProperty(nameof(HealthPlan.IsActive));
            Assert.NotNull(planIsActiveProp);
            Assert.True((bool?)planIsActiveProp!.GetDefaultValue() ?? false);
        }
    }
}
