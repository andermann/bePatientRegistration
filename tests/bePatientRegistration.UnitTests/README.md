# bePatientRegistration.UnitTests – Domain & Infrastructure

## Objetivo

Garantir **cobertura de testes ≥ 85%** nos projetos:

- `bePatientRegistration.Domain`
- `bePatientRegistration.Infrastructure`

Validando:

- Regras de negócio das **Entities** (`Patient`, `HealthPlan`)
- Regras e validações dos **ValueObjects** (`Cpf`, `Email`, `PhoneNumber`)
- Configuração de **mapeamentos EF Core** (tabelas, max length, default values, relacionamentos)
- Funcionamento dos **repositórios** (`PatientRepository`, `HealthPlanRepository`) usando **EFCore InMemory**

Os testes são escritos com **xUnit** seguindo o padrão **AAA (Arrange, Act, Assert)**.

---

## Estrutura do projeto de testes

```text
bePatientRegistration.sln
├── src/
│   ├── bePatientRegistration.Domain/
│   ├── bePatientRegistration.Infrastructure/
│   └── ...
└── tests/
    ├── bePatientRegistration.UnitTests/
    │   ├── bePatientRegistration.UnitTests.csproj
    │   ├── Domain/
    │   │   ├── Entities/
    │   │   │   ├── HealthPlanTests.cs
    │   │   │   └── PatientTests.cs
    │   │   └── ValueObjects/
    │   │       ├── CpfTests.cs
    │   │       ├── EmailTests.cs
    │   │       └── PhoneNumberTests.cs
    │   └── Infrastructure/
    │       ├── Persistence/
    │       │   └── ApplicationDbContextTests.cs
    │       └── Repositories/
    │           ├── HealthPlanRepositoryTests.cs
    │           └── PatientRepositoryTests.cs
    └── bePatientRegistration.IntegrationTests/
