//using System;
//using System.Linq;
//using bePatientRegistration.Domain.Entities;
//using bePatientRegistration.Domain.ValueObjects;
//using bePatientRegistration.Infrastructure.Persistence;

//namespace bePatientRegistration.IntegrationTests
//{
//    public static class IntegrationTestDataSeeder
//    {
//        public static void Seed(ApplicationDbContext context)
//        {
//            // Convênios básicos
//            if (!context.HealthPlans.Any())
//            {
//                var basic = new HealthPlan("Plano Básico Integração");
//                var premium = new HealthPlan("Plano Premium Integração");

//                context.HealthPlans.AddRange(basic, premium);
//                context.SaveChanges();
//            }

//            // Paciente de exemplo
//            if (!context.Patients.Any())
//            {
//                var healthPlan = context.HealthPlans.First();

//                var patient = new Patient(
//                    firstName: "João",
//                    lastName: "Silva",
//                    dateOfBirth: new DateTime(1990, 1, 1),
//                    gender: Gender.Male,
//                    cpf: "19575446003",
//                    rg: "RJ123456",
//                    ufRg: Uf.RJ,
//                    email: "joao.silva@example.com",
//                    mobilePhone: "21999990000",
//                    landlinePhone: null,
//                    healthPlanId: healthPlan.Id,
//                    healthPlanCardNumber: "CARD-0001",
//                    healthPlanCardExpirationMonth: 12,
//                    healthPlanCardExpirationYear: 2030
//                );

//                context.Patients.Add(patient);
//                context.SaveChanges();
//            }
//        }
//    }
//}

using System;
using System.Linq;
using bePatientRegistration.Domain.Entities;
using bePatientRegistration.Domain.ValueObjects;
using bePatientRegistration.Infrastructure.Persistence;

namespace bePatientRegistration.IntegrationTests
{
    public static class IntegrationTestDataSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Convênios básicos
            if (!context.HealthPlans.Any())
            {
                var basic = new HealthPlan("Plano Básico Integração");
                var premium = new HealthPlan("Plano Premium Integração");

                context.HealthPlans.AddRange(basic, premium);
                context.SaveChanges();
            }

            // Paciente de exemplo
            if (!context.Patients.Any())
            {
                var healthPlan = context.HealthPlans.First();

                // CPF precisa ser VÁLIDO conforme o ValueObject Cpf
                var patient = new Patient(
                    firstName: "João",
                    lastName: "Silva",
                    dateOfBirth: new DateTime(1990, 1, 1),
                    gender: Gender.Male,
                    cpf: "52998224725",              // <-- CPF válido
                    rg: "RJ123456",
                    ufRg: Uf.RJ,
                    email: "joao.silva@example.com",
                    mobilePhone: "21999990000",
                    landlinePhone: null,
                    healthPlanId: healthPlan.Id,
                    healthPlanCardNumber: "CARD-0001",
                    healthPlanCardExpirationMonth: 12,
                    healthPlanCardExpirationYear: 2030
                );

                context.Patients.Add(patient);
                context.SaveChanges();
            }
        }
    }
}
