using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Infrastructure.Persistence;
using bePatientRegistration.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace bePatientRegistration.UnitTests.Infrastructure.Repositories
{
    public class HealthPlanRepositoryTests
    {
        private static ApplicationDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddAsync_Should_AddHealthPlan_ToContext()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(AddAsync_Should_AddHealthPlan_ToContext));
            var repository = new HealthPlanRepository(context);
            var plan = new HealthPlan("Plano Ouro");

            // Act
            await repository.AddAsync(plan);
            await repository.SaveChangesAsync();

            // Assert
            var fromDb = await context.HealthPlans.FindAsync(plan.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("Plano Ouro", fromDb!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnHealthPlan_WhenExists()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(GetByIdAsync_Should_ReturnHealthPlan_WhenExists));
            var existingPlan = new HealthPlan("Plano Prata");

            await context.HealthPlans.AddAsync(existingPlan);
            await context.SaveChangesAsync();

            var repository = new HealthPlanRepository(context);

            // Act
            var result = await repository.GetByIdAsync(existingPlan.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingPlan.Id, result!.Id);
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnAllHealthPlans()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(GetAllAsync_Should_ReturnAllHealthPlans));
            await context.HealthPlans.AddRangeAsync(
                new HealthPlan("Plano A"),
                new HealthPlan("Plano B")
            );
            await context.SaveChangesAsync();

            var repository = new HealthPlanRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateHealthPlan()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(UpdateAsync_Should_UpdateHealthPlan));
            var plan = new HealthPlan("Plano Antigo");
            await context.HealthPlans.AddAsync(plan);
            await context.SaveChangesAsync();

            var repository = new HealthPlanRepository(context);

            // Act
            plan.Deactivate();
            await repository.UpdateAsync(plan);
            await repository.SaveChangesAsync();

            // Assert
            var reloaded = await context.HealthPlans.FindAsync(plan.Id);
            Assert.NotNull(reloaded);
            Assert.False(reloaded!.IsActive);
        }

        [Fact]
        public async Task ExistsByIdAsync_Should_ReturnTrue_WhenPlanExists()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(ExistsByIdAsync_Should_ReturnTrue_WhenPlanExists));
            var plan = new HealthPlan("Plano Teste");
            await context.HealthPlans.AddAsync(plan);
            await context.SaveChangesAsync();

            var repository = new HealthPlanRepository(context);

            // Act
            var exists = await repository.ExistsByIdAsync(plan.Id);
            var notExists = await repository.ExistsByIdAsync(Guid.NewGuid());

            // Assert
            Assert.True(exists);
            Assert.False(notExists);
        }

        [Fact]
        public async Task ExistsByNameAsync_Should_ReturnTrue_WhenPlanExists()
        {
            // Arrange
            using var context = CreateInMemoryContext(nameof(ExistsByNameAsync_Should_ReturnTrue_WhenPlanExists));
            var plan = new HealthPlan("Plano Único");
            await context.HealthPlans.AddAsync(plan);
            await context.SaveChangesAsync();

            var repository = new HealthPlanRepository(context);

            // Act
            var exists = await repository.ExistsByNameAsync("Plano Único");
            var notExists = await repository.ExistsByNameAsync("Outro Plano");

            // Assert
            Assert.True(exists);
            Assert.False(notExists);
        }
    }
}
