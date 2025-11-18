//using System.Linq;
//using bePatientRegistration.Domain.Entities;
//using bePatientRegistration.Infrastructure.Persistence;
//using bePatientRegistration.Infrastructure.Persistence.Seed;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;

//namespace bePatientRegistration.UnitTests.Infrastructure.Persistence;

//public class ApplicationDbSeederTests
//{
//    private static (SqliteConnection connection, ApplicationDbContext context) CreateSqliteContext()
//    {
//        // Banco SQLite em memória compartilhada pela conexão
//        var connection = new SqliteConnection("DataSource=:memory:");
//        connection.Open();

//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseSqlite(connection)
//            .Options;

//        var context = new ApplicationDbContext(options);

//        return (connection, context);
//    }

//    [Fact]
//    public void Seed_Should_CreateDatabase_And_InsertDefaultHealthPlans_When_Empty()
//    {
//        // Arrange
//        var (connection, context) = CreateSqliteContext();

//        try
//        {
//            // Act
//            ApplicationDbSeeder.Seed(context);

//            // Assert
//            var plans = context.HealthPlans.ToList();
//            Assert.NotEmpty(plans);
//            Assert.True(plans.Count >= 5);

//            var expectedNames = new[]
//            {
//                "Amil Saúde",
//                "Bradesco Saúde",
//                "Unimed Nacional",
//                "SulAmérica Saúde",
//                "Golden Cross"
//            };

//            foreach (var name in expectedNames)
//            {
//                Assert.Contains(plans, p => p.Name == name);
//            }
//        }
//        finally
//        {
//            context.Dispose();
//            connection.Close();
//            connection.Dispose();
//        }
//    }

//    [Fact]
//    public void Seed_Should_Not_DuplicateHealthPlans_When_CalledTwice()
//    {
//        // Arrange
//        var (connection, context) = CreateSqliteContext();

//        try
//        {
//            // Act
//            ApplicationDbSeeder.Seed(context);
//            var firstCount = context.HealthPlans.Count();

//            ApplicationDbSeeder.Seed(context);
//            var secondCount = context.HealthPlans.Count();

//            // Assert
//            Assert.Equal(firstCount, secondCount);
//        }
//        finally
//        {
//            context.Dispose();
//            connection.Close();
//            connection.Dispose();
//        }
//    }
//}

using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Infrastructure.Persistence;
using bePatientRegistration.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace bePatientRegistration.UnitTests.Infrastructure.Persistence
{
    public class ApplicationDbSeederTests
    {
        private static ApplicationDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public void Seed_Should_CreateSchema_And_InsertDefaultHealthPlans_When_Empty()
        {
            // Arrange
            using var context = CreateInMemoryContext(
                nameof(Seed_Should_CreateSchema_And_InsertDefaultHealthPlans_When_Empty));

            // Act
            ApplicationDbSeeder.Seed(context);

            // Assert
            var plans = context.HealthPlans.ToList();
            Assert.NotEmpty(plans);
            Assert.True(plans.Count >= 5);

            var expectedNames = new[]
            {
                "Amil Saúde",
                "Bradesco Saúde",
                "Unimed Nacional",
                "SulAmérica Saúde",
                "Golden Cross"
            };

            foreach (var name in expectedNames)
            {
                Assert.Contains(plans, p => p.Name == name);
            }
        }

        [Fact]
        public void Seed_Should_Not_DuplicateHealthPlans_When_CalledTwice()
        {
            // Arrange
            using var context = CreateInMemoryContext(
                nameof(Seed_Should_Not_DuplicateHealthPlans_When_CalledTwice));

            // Act
            ApplicationDbSeeder.Seed(context);
            var firstCount = context.HealthPlans.Count();

            ApplicationDbSeeder.Seed(context);
            var secondCount = context.HealthPlans.Count();

            // Assert
            Assert.Equal(firstCount, secondCount);
        }
    }
}

