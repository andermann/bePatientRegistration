# bePatientRegistration

## API + Front-end Angular para cadastro de pacientes e convênios — Desafio BE3.

---

# 📌 **1. Arquitetura Utilizada**

Este projeto segue os princípios de **Clean Architecture**, dividindo a solução em camadas bem definidas para garantir baixo acoplamento, testabilidade e organização.

### **Camadas**

| Camada                | Objetivo                                                                          |
| --------------------- | --------------------------------------------------------------------------------- |
| **Domain**            | Entidades, regras de negócio puras e contratos essenciais. Não depende de nada.   |
| **Application**       | DTOs, validações, serviços de aplicação e casos de uso. Depende apenas da Domain. |
| **Infrastructure**    | Persistência (EF Core), Migrations, Repositórios concretos.                       |
| **API**               | Entrada da aplicação. Controllers, configuração, DI e Middlewares.                |
| **Angular Front-End** | SPA para cadastro/listagem de Pacientes e Convênios.                              |

### **Fluxo Geral**

API → Application → Domain → Infrastructure → Banco (SQL Server)

---

# 📌 **2. Boas Práticas Aplicadas**

As seguintes práticas foram aplicadas:

* ✔ **Clean Architecture**
* ✔ **SOLID**
* ✔ **Repository Pattern**
* ✔ **DTOs + Validators (FluentValidation)**
* ✔ **Separação clara entre camadas**
* ✔ **Migrations versionadas**
* ✔ **Seeder para dados iniciais (Convênios)**
* ✔ **Controllers enxutos**
* ✔ **CORS configurado para Angular**
* ✔ **Swagger sempre habilitado para desenvolvimento**
* ✔ **Uso de async/await em todos os endpoints**
* ✔ **Tratamento de erros via validações e responses claros**

---

# 📌 **3. Tecnologias Utilizadas**

### **Back-end (.NET 8)**

* ASP.NET Core 8 Web API
* Entity Framework Core 8
* Microsoft SQL Server (rodando via Docker)
* FluentValidation
* Swagger / Swashbuckle
* C# 12
* Microsoft.Extensions.DependencyInjection

### **Front-end**

* Angular 17 (standalone components)
* RxJS
* Bootstrap
* Bootstrap Icons
* ngx-mask

---

# 📌 **4. Referências entre Projetos da Solução (.csproj)**

> **Use estes comandos somente se estiver configurando a solução do zero.**

## 📌 API → precisa de Application + Infrastructure

```bash
dotnet add src/bePatientRegistration.Api/bePatientRegistration.Api.csproj reference src/bePatientRegistration.Application/bePatientRegistration.Application.csproj
dotnet add src/bePatientRegistration.Api/bePatientRegistration.Api.csproj reference src/bePatientRegistration.Infrastructure/bePatientRegistration.Infrastructure.csproj
```

## 📌 Infrastructure → precisa de Domain

```bash
dotnet add src/bePatientRegistration.Infrastructure/bePatientRegistration.Infrastructure.csproj reference src/bePatientRegistration.Domain/bePatientRegistration.Domain.csproj
```

## 📌 Application → precisa de Domain

```bash
dotnet add src/bePatientRegistration.Application/bePatientRegistration.Application.csproj reference src/bePatientRegistration.Domain/bePatientRegistration.Domain.csproj
```

---

# 📌 **5. Instalação dos Pacotes NuGet**

Use na raiz da solução ou navegue até cada `.csproj`.

```bash
dotnet add package Microsoft.CodeCoverage --version 18.0.1
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.5
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.5
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.5
dotnet add package Microsoft.EntityFrameworkCore.Relational --version 8.0.5
dotnet add package Microsoft.VisualStudio.Azure.Containers.Tools.Targets --version 1.22.1
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.2
dotnet add package Swashbuckle.AspNetCore --version 6.6.2
dotnet add package FluentValidation.AspNetCore
dotnet add package FluentValidation
```

---

# 📌 **6. Banco de Dados no Docker (SQL Server)**

## ▶️ **Criar container do SQL Server**

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_strong_Password123" -p 1400:1433 -d mcr.microsoft.com/mssql/server:2025-latest
```

---

## ▶️ Criar banco de dados dentro do container

```powershell
$DatabaseName = "BePatientRegistrationDb"
$SQLCommand = "CREATE DATABASE $DatabaseName;"

sqlcmd -S 127.0.0.1,1400 `
       -U sa `
       -P Your_strong_Password123 `
       -Q $SQLCommand
```

---

# 📌 **7. Executar Migrations**

## Criar migration inicial (caso ainda não exista)

```bash
dotnet ef migrations add InitialCreate `
  -p ./src/bePatientRegistration.Infrastructure/bePatientRegistration.Infrastructure.csproj `
  -s ./src/bePatientRegistration.Api/bePatientRegistration.Api.csproj `
  -o Persistence/Migrations
```

## Aplicar migration

```bash
dotnet ef database update `
  -p ./src/bePatientRegistration.Infrastructure/bePatientRegistration.Infrastructure.csproj `
  -s ./src/bePatientRegistration.Api/bePatientRegistration.Api.csproj
```

---

# 📌 **8. Popular Banco Automaticamente (Seeder)**

O `ApplicationDbSeeder` roda na inicialização da API e popula a tabela `HealthPlans` caso esteja vazia.

Convênios inseridos:

1. Amil Saúde
2. Bradesco Saúde
3. Unimed Nacional
4. SulAmérica Saúde
5. Golden Cross

Nada a fazer — é automático.

---

# 📌 **9. Executar a API**

Na raiz:

```bash
dotnet build
dotnet run --project src/bePatientRegistration.Api/bePatientRegistration.Api.csproj
```

Abrir Swagger:

👉 [http://localhost:5000/swagger](http://localhost:5000/swagger)
👉 ou [http://localhost:5206/swagger](http://localhost:5206/swagger) (porta usada pelo VS)

---

# 📌 **10. Executar o Front-End Angular**

```bash
cd frontend
npm install
ng serve -o
```

O front está configurado para chamar a API em:

```
http://localhost:5206
```

---

# 📌 **11. Estrutura da Solução**

```
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

# 📌 **12. Conclusão**

Este README entrega:

✔ Instruções de instalação
✔ Execução da API
✔ Execução do Angular
✔ Como rodar banco e popular base
✔ Docker
✔ Arquitetura
✔ Boas práticas
✔ Lista de tecnologias
✔ Scripts de referência e instalação

Se quiser, posso gerar também:

✅ versão em **PDF**
✅ versão em **DOCX**
✅ adicionando logos e layout profissional

Só pedir!
