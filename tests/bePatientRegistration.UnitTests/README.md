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
```

# bePatientRegistration.UnitTests – Domain, Infrastructure & Application

## Objetivo

Garantir **cobertura de testes ≥ 85%** nos projetos:

- `bePatientRegistration.Domain`
- `bePatientRegistration.Infrastructure`
- `bePatientRegistration.Application`

Validando:

- Regras de negócio das **Entities** (`Patient`, `HealthPlan`);
- Regras e validações dos **ValueObjects** (`Cpf`, `Email`, `PhoneNumber`);
- Configuração de **mapeamentos EF Core** (tabelas, tamanhos, relacionamentos);
- Funcionamento dos **repositórios** (`PatientRepository`, `HealthPlanRepository`) usando **EF Core InMemory**;
- Regras de negócio da **Application**:
  - `HealthPlanAppService`
  - `PatientAppService`
  - `CreatePatientRequestValidator`
  - `UpdatePatientRequestValidator`

---

## Tecnologias e NuGets utilizados

Projeto: `tests/bePatientRegistration.UnitTests/bePatientRegistration.UnitTests.csproj`

Pacotes instalados:

- `Microsoft.NET.Test.Sdk` – infraestrutura de testes para `dotnet test`;
- `xunit` – framework de testes unitários;
- `xunit.runner.visualstudio` – integração com Test Explorer (VS / Rider);
- `Moq` – criação de *mocks* para repositórios e serviços;
- `Microsoft.EntityFrameworkCore` – base para uso em testes de infraestrutura;
- `Microsoft.EntityFrameworkCore.InMemory` – provider em memória para testar o EF Core sem banco real;
- `coverlet.collector` – coleta de cobertura (`--collect:"XPlat Code Coverage"`);
- *(Opcional / comentado no .csproj)* `Microsoft.Data.Sqlite` – para cenários de teste com banco em arquivo.

### Script para reinstalar os pacotes (na pasta `tests/bePatientRegistration.UnitTests`)

``` bash
dotnet add package Microsoft.NET.Test.Sdk --version 18.0.0
dotnet add package xunit --version 2.9.2
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package Moq --version 4.20.72
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.5
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 9.0.5
dotnet add package coverlet.collector --version 6.0.2

# opcional:
# dotnet add package Microsoft.Data.Sqlite --version 9.0.0
``` 
---

## Estrutura do projeto de testes
```text
tests/
  bePatientRegistration.UnitTests/
    bePatientRegistration.UnitTests.csproj
    README.md

    Domain/
      Entities/
        HealthPlanTests.cs
        PatientTests.cs
      ValueObjects/
        CpfTests.cs
        EmailTests.cs
        PhoneNumberTests.cs

    Infrastructure/
      Persistence/
        ApplicationDbContextTests.cs
        ApplicationDbSeederTests.cs
      Repositories/
        HealthPlanRepositoryTests.cs
        PatientRepositoryTests.cs

    Application/
      HealthPlans/
        Services/
          HealthPlanAppServiceTests.cs
      Patients/
        Services/
          PatientAppServiceTests.cs
        Validators/
          CreatePatientRequestValidatorTests.cs
          UpdatePatientRequestValidatorTests.cs
```



