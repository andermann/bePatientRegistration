using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Infrastructure.Persistence;
using bePatientRegistration.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace bePatientRegistration.UnitTests.Infrastructure.Repositories
{
    public class PatientRepositoryTests
    {
        private static ApplicationDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        private static Patient CreateValidPatient(Guid? healthPlanId = null, string cpf = "12345678909")
        {
            var dob = DateTime.Today.AddYears(-30);
            var expiration = DateTime.Today.AddMonths(1);
            var planId = healthPlanId ?? Guid.NewGuid();

            return new Patient(
                firstName: "João",
                lastName: "Silva",
                dateOfBirth: dob,
                gender: Gender.Male,
                cpf: cpf,
                rg: "1234567",
                ufRg: Uf.RJ,
                email: "joao.silva@example.com",
                mobilePhone: "21999990000",
                landlinePhone: null,
                healthPlanId: planId,
                healthPlanCardNumber: "ABC123456",
                healthPlanCardExpirationMonth: expiration.Month,
                healthPlanCardExpirationYear: expiration.Year
            );
        }

        [Fact]
        public async Task AddAsync_Should_AddPatient_ToContext()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(AddAsync_Should_AddPatient_ToContext));
            var repository = new PatientRepository(context);

            var plan = new HealthPlan("Plano Teste");
            await context.HealthPlans.AddAsync(plan);
            await context.SaveChangesAsync();

            var patient = CreateValidPatient(healthPlanId: plan.Id);

            // Act
            await repository.AddAsync(patient);
            await repository.SaveChangesAsync();

            // Assert
            var fromDb = await context.Patients.FindAsync(patient.Id);
            Assert.NotNull(fromDb);
            Assert.Equal(patient.Id, fromDb!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnPatient_WhenExists()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(GetByIdAsync_Should_ReturnPatient_WhenExists));
            var plan = new HealthPlan("Plano Teste");
            await context.HealthPlans.AddAsync(plan);

            var patient = CreateValidPatient(healthPlanId: plan.Id);
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();

            var repository = new PatientRepository(context);

            // Act
            var result = await repository.GetByIdAsync(patient.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patient.Id, result!.Id);
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnAllPatients()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(GetAllAsync_Should_ReturnAllPatients));
            var plan = new HealthPlan("Plano Teste");
            await context.HealthPlans.AddAsync(plan);

            var patient1 = CreateValidPatient(healthPlanId: plan.Id);
            var patient2 = CreateValidPatient(healthPlanId: plan.Id);

            await context.Patients.AddRangeAsync(patient1, patient2);
            await context.SaveChangesAsync();

            var repository = new PatientRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdatePatient()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(UpdateAsync_Should_UpdatePatient));
            var plan = new HealthPlan("Plano Teste");
            await context.HealthPlans.AddAsync(plan);

            var patient = CreateValidPatient(healthPlanId: plan.Id);
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();

            var repository = new PatientRepository(context);

            // Act
            patient.SetName("Maria", "Souza");
            await repository.UpdateAsync(patient);
            await repository.SaveChangesAsync();

            // Assert
            var reloaded = await context.Patients.FindAsync(patient.Id);
            Assert.NotNull(reloaded);
            Assert.Equal("Maria", reloaded!.FirstName);
            Assert.Equal("Souza", reloaded.LastName);
        }

        [Fact]
        public async Task DeleteAsync_Should_RemovePatient_FromContext()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(DeleteAsync_Should_RemovePatient_FromContext));
            var plan = new HealthPlan("Plano Teste");
            await context.HealthPlans.AddAsync(plan);

            var patient = CreateValidPatient(healthPlanId: plan.Id);
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();

            var repository = new PatientRepository(context);

            // Act
            await repository.DeleteAsync(patient);
            await repository.SaveChangesAsync();

            // Assert
            var fromDb = await context.Patients.FindAsync(patient.Id);
            //Assert.Null(fromDb);
            Assert.True(fromDb.IsActive == false);
        }

        [Fact]
        public async Task ExistsByIdAsync_Should_ReturnTrue_WhenPatientExists()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(ExistsByIdAsync_Should_ReturnTrue_WhenPatientExists));
            var plan = new HealthPlan("Plano Teste");
            await context.HealthPlans.AddAsync(plan);

            var patient = CreateValidPatient(healthPlanId: plan.Id);
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();

            var repository = new PatientRepository(context);

            // Act
            var exists = await repository.ExistsByIdAsync(patient.Id);
            var notExists = await repository.ExistsByIdAsync(Guid.NewGuid());

            // Assert
            Assert.True(exists);
            Assert.False(notExists);
        }

        [Fact]
        public async Task ExistsByCpfAsync_Should_ValidateCpf()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(ExistsByCpfAsync_Should_ValidateCpf));
            var plan = new HealthPlan("Plano Teste");
            await context.HealthPlans.AddAsync(plan);

            var patient = CreateValidPatient(healthPlanId: plan.Id, cpf: "12345678909");
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();

            var repository = new PatientRepository(context);

            // Act
            var exists = await repository.ExistsByCpfAsync("12345678909");
            var notExists = await repository.ExistsByCpfAsync("00000000000");

            // Assert
            Assert.True(exists);
            Assert.False(notExists);
        }
    }
}
