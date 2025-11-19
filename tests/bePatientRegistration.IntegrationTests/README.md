# bePatientRegistration.IntegrationTests

Projeto de **Testes de Integração** da API `bePatientRegistration.Api`.

O objetivo é validar o pipeline real:

- Controllers
- Serviços (Application)
- Infraestrutura (Repositories, DbContext)
- EF Core
- Serialização
- Contratos HTTP

Usando **WebApplicationFactory + TestServer** com banco **InMemory**.

---

# 1. Tecnologias utilizadas

| Tecnologia | Uso |
|-----------|-----|
| **xUnit** | Framework de teste |
| **Microsoft.AspNetCore.Mvc.Testing** | Criação do TestServer + WebApplicationFactory |
| **Microsoft.AspNetCore.TestHost** | Ambiente WebHost fake p/ integração |
| **Microsoft.EntityFrameworkCore.InMemory** | Banco de dados em memória |
| **System.Net.Http.Json** | Serialização/deserialização JSON |
| **coverlet.collector** | Coleta de cobertura de código |
| **Microsoft.NET.Test.Sdk** | Suporte ao run de testes |

---

# 2. Pacotes NuGet Necessários

Execute dentro de:

    tests/bePatientRegistration.IntegrationTests/


## Pacotes obrigatórios:

```bash
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.0
dotnet add package Microsoft.AspNetCore.TestHost --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.0
dotnet add package System.Net.Http.Json
dotnet add package Microsoft.Extensions.Hosting --version 8.0.0
dotnet add package Microsoft.Extensions.DependencyInjection --version 8.0.0
```

## Pacotes de teste:

```
dotnet add package Microsoft.NET.Test.Sdk --version 17.10.0
dotnet add package xunit --version 2.8.0
dotnet add package xunit.runner.visualstudio --version 2.8.0
dotnet add package coverlet.collector --version 6.0.0
```

## Estrutura
```
bePatientRegistration.IntegrationTests
 ├── ApiFactory.cs
 ├── IntegrationTestDataSeeder.cs
 ├── HealthPlansEndpointsTests.cs
 ├── PatientsEndpointsTests.cs
 ├── README.md
```

