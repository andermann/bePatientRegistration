# bePatientRegistration

API + Front-end Angular para cadastro de pacientes e convênios — Desafio BE3.

---

## 1. Arquitetura Utilizada

Este projeto segue os princípios de **Clean Architecture**, dividindo a solução em camadas bem definidas para garantir baixo acoplamento, testabilidade e organização.

### 1.1. Camadas

| Camada                | Objetivo                                                                          |
| --------------------- | --------------------------------------------------------------------------------- |
| **Domain**            | Entidades, regras de negócio puras e contratos essenciais. Não depende de nada.  |
| **Application**       | DTOs, validações, serviços de aplicação e casos de uso. Depende apenas da Domain.|
| **Infrastructure**    | Persistência (EF Core), Migrations, Repositórios concretos.                       |
| **API**               | Entrada da aplicação. Controllers, configuração, DI e Middlewares.               |
| **Angular Front-End** | SPA para cadastro/listagem de Pacientes e Convênios.                             |

### 1.2. Fluxo Geral

API → Application → Domain → Infrastructure → Banco (SQL Server)

---

## 2. Boas Práticas Aplicadas

As seguintes práticas foram aplicadas:

- ✔ **Clean Architecture**
- ✔ **SOLID**
- ✔ **Repository Pattern**
- ✔ **DTOs + Validators (FluentValidation)**
- ✔ **Separação clara entre camadas**
- ✔ **Migrations versionadas**
- ✔ **Seeder para dados iniciais (Convênios)**
- ✔ **Controllers enxutos**
- ✔ **CORS configurado para Angular**
- ✔ **Swagger sempre habilitado para desenvolvimento**
- ✔ **Uso de async/await em todos os endpoints**
- ✔ **Tratamento de erros via validações e responses claros**

---

## 3. Tecnologias Utilizadas

### 3.1. Back-end (.NET 8)

- ASP.NET Core 8 Web API  
- Entity Framework Core 8  
- Microsoft SQL Server (rodando via Docker)  
- FluentValidation  
- Swagger / Swashbuckle  
- C# 12  
- Microsoft.Extensions.DependencyInjection  

### 3.2. Front-end

- Angular 17 (standalone components)  
- RxJS  
- Bootstrap  
- Bootstrap Icons  
- ngx-mask  

---

## 4. Configuração por Projeto (.NET)

> Use estes comandos somente se estiver configurando a solução do zero ou precisando recriar referências/pacotes.

A estrutura considerada é:

```text
src/
  bePatientRegistration.Api/
  bePatientRegistration.Application/
  bePatientRegistration.Domain/
  bePatientRegistration.Infrastructure/
````

### 4.1. Projeto: bePatientRegistration.Domain

#### 4.1.1. Referências de Projeto

A camada **Domain** é a raiz da arquitetura: não deve depender de nenhum outro projeto.

```bash
# Nenhuma referência de projeto necessária
```

#### 4.1.2. Pacotes NuGet

Idealmente, o Domain não deve depender de nada externo. Se quiser manter o domínio 100% puro:

```bash
# Nenhum pacote obrigatório no Domain
```

---

### 4.2. Projeto: bePatientRegistration.Application

#### 4.2.1. Referências de Projeto

O **Application** depende apenas do **Domain**:

```bash
dotnet add bePatientRegistration.Application.csproj reference ..\bePatientRegistration.Domain\bePatientRegistration.Domain.csproj
```

#### 4.2.2. Pacotes NuGet

Pacotes típicos utilizados na camada de aplicação (DTOs, casos de uso, validações):

```bash
dotnet add bePatientRegistration.Application.csproj package AutoMapper --version 15.0.1
dotnet add bePatientRegistration.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.0
dotnet add bePatientRegistration.Application.csproj package FluentValidation --version 12.1.0
```

---

### 4.3. Projeto: bePatientRegistration.Infrastructure

#### 4.3.1. Referências de Projeto

O **Infrastructure** implementa repositórios e acesso a dados, dependendo do **Domain**:

```bash
dotnet add bePatientRegistration.Infrastructure.csproj reference ..\bePatientRegistration.Application\bePatientRegistration.Application.csproj
dotnet add bePatientRegistration.Infrastructure.csproj reference ..\bePatientRegistration.Domain\bePatientRegistration.Domain.csproj
```

#### 4.3.2. Pacotes NuGet

Pacotes relacionados à persistência (Entity Framework Core + SQL Server):

```bash
dotnet add bePatientRegistration.Infrastructure.csproj package Microsoft.EntityFrameworkCore --version 8.0.8
dotnet add bePatientRegistration.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.8
dotnet add bePatientRegistration.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.8
dotnet add bePatientRegistration.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools --version 8.0.8
dotnet add bePatientRegistration.Infrastructure.csproj package Microsoft.Extensions.Configuration --version 9.0.6
```

```bash
# Opcional – apenas se usar InMemory aqui
dotnet add src/bePatientRegistration.Infrastructure/bePatientRegistration.Infrastructure.csproj package Microsoft.EntityFrameworkCore.InMemory
```

---

### 4.4. Projeto: bePatientRegistration.Api

#### 4.4.1. Referências de Projeto

A API precisa da camada de aplicação e da infraestrutura:

```bash
dotnet add bePatientRegistration.Api.csproj reference ..\bePatientRegistration.Application\bePatientRegistration.Application.csproj
dotnet add bePatientRegistration.Api.csproj reference ..\bePatientRegistration.Infrastructure\bePatientRegistration.Infrastructure.csproj
```

#### 4.4.2. Pacotes NuGet

Pacotes típicos utilizados no projeto da API:

```bash
dotnet add bePatientRegistration.Api.csproj package FluentValidation.AspNetCore --version 11.3.1
dotnet add bePatientRegistration.Api.csproj package Microsoft.EntityFrameworkCore --version 8.0.8
dotnet add bePatientRegistration.Api.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.8
dotnet add bePatientRegistration.Api.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.8
dotnet add bePatientRegistration.Api.csproj package Swashbuckle.AspNetCore --version 10.0.1
```

---

## 5. Banco de Dados no Docker e Migrations

### 5.1. Subir SQL Server via Docker

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_strong_Password123" -p 1400:1433 -d mcr.microsoft.com/mssql/server:2025-latest
```

### 5.2. Criar banco de dados dentro do container

``` powershell
$DatabaseName = "BePatientRegistrationDb"
$SQLCommand = "CREATE DATABASE $DatabaseName;"

sqlcmd -S 127.0.0.1,1400 `
       -U sa `
       -P Your_strong_Password123 `
       -Q $SQLCommand
```

### 5.3. Criar e aplicar migrations

#### 5.3.1. Criar migration inicial (caso ainda não exista)

```bash
dotnet ef migrations add InitialCreate `
  -p ./src/bePatientRegistration.Infrastructure/bePatientRegistration.Infrastructure.csproj `
  -s ./src/bePatientRegistration.Api/bePatientRegistration.Api.csproj `
  -o Persistence/Migrations
```

#### 5.3.2. Aplicar migration

```bash
dotnet ef database update `
  -p ./src/bePatientRegistration.Infrastructure/bePatientRegistration.Infrastructure.csproj `
  -s ./src/bePatientRegistration.Api/bePatientRegistration.Api.csproj
```

---

## 6. Popular Banco Automaticamente (Seeder)

O `ApplicationDbSeeder` roda na inicialização da API e popula a tabela `HealthPlans` caso esteja vazia.

Convênios inseridos automaticamente:

1. Amil Saúde
2. Bradesco Saúde
3. Unimed Nacional
4. SulAmérica Saúde
5. Golden Cross

> Não é necessário rodar script manual de insert — basta subir a API com o banco vazio.

---

## 7. Executar a API

Na raiz da solução:

```bash
dotnet build
dotnet run --project src/bePatientRegistration.Api/bePatientRegistration.Api.csproj

Abrir Swagger(Documentação back-end):

    http://localhost:5234/swagger (porta usada pelo Visual Studio)
Abrir Front-end em:
    http://localhost:4000/ 

```

---

## 8. Executar o Front-End Angular

```bash
cd be-patient-registration
npm install
ng serve -o
```

O front está configurado para chamar a API em:

```text
http://localhost:5234
```

(Ajuste a URL no `environment.ts` caso altere a porta da API.)

---

## 10. Execução Conjunta (API + Front)

```
rodar .\run-api-and-front.ps1, na rais da solução, para subir API + Front juntos.

```



## 11. Estrutura da Solução

```text
bePatientRegistration/
│
├── src/
│   ├── bePatientRegistration.Api/
│   ├── bePatientRegistration.Application/
│   ├── bePatientRegistration.Domain/
│   ├── bePatientRegistration.Infrastructure/
│   └── frontend/
│
├── tests/
│   └── (caso adicione testes futuramente)
│
└── README.md
```

---

## 12. Conclusão

Este README entrega:

* Instruções de instalação e execução
* Como criar o container Docker e o banco
* Como rodar migrations e popular a base (Seeder)
* Descrição da arquitetura usada
* Boas práticas adotadas
* Tecnologias principais
* Scripts de referências entre projetos **por projeto**
* Scripts de instalação de pacotes NuGet **por projeto**
